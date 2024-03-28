using MediatR;
using BackEnd.Abstracts;
using System.Security.Claims;
using BackEnd.BusinessLogic.Tenant;
using BackEnd.BusinessLogic.FarmLocation;
using BackEnd.BusinessLogic.Livestock;
using BackEnd.BusinessLogic.Livestock.Animals;
using BackEnd.BusinessLogic.Livestock.Breeds;
using BackEnd.BusinessLogic.Livestock.Status;
using BackEnd.BusinessLogic.Duty;
using BackEnd.BusinessLogic.ScheduledDuty;
using BackEnd.BusinessLogic.Milestone;
using BackEnd.BusinessLogic.Event;
using BackEnd.BusinessLogic.ManyToMany;
using BackEnd.BusinessLogic.BreedingRecord;
using BackEnd.BusinessLogic.Registrar;
using BackEnd.BusinessLogic.Registration;
using BackEnd.BusinessLogic.Unit;
using BackEnd.BusinessLogic.Measure;
using BackEnd.BusinessLogic.FarmLocation.LandPlots;
using BackEnd.BusinessLogic.Livestock.Feed;
using BackEnd.BusinessLogic.Treatment;
using BackEnd.BusinessLogic.TreatmentRecord;
using BackEnd.BusinessLogic.Measurement;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Domain.ValueObjects;
using BackEnd.BusinessLogic.Chore;

namespace MicroAgManager.API
{
    public static class BusinessLogicAPI
    {

        public static void MapCustomRegistration(WebApplication app)
        {
            // provide an end point to clear the cookie for logout
            // NOTE: This logout code will be updated shortly.
            //       https://github.com/dotnet/blazor-samples/issues/132
            app.MapGroup("/account").MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.Ok();
            });
            app.MapGroup("/account").MapGet("/userAuthenticated", async Task<Results<Ok<UserInfo>, ValidationProblem, NotFound>>
            (ClaimsPrincipal claimsPrincipal, UserManager<ApplicationUser> userManager) =>
            {
                if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
                {
                    return TypedResults.NotFound();
                }
                return TypedResults.Ok(new UserInfo { 
                    Email = user.Email, 
                    TenantId = user.TenantId.ToString(), 
                    UserId = user.Id.ToString(), 
                    IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
                    Claims=claimsPrincipal.Claims.ToDictionary(claim => claim.Type, claim => claim.Value)
                });
            });
            app.MapGroup("/account").MapPost("/customregister", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] RegisterRequest registration, HttpContext context,
            UserManager < ApplicationUser > userManager,
            IUserStore < ApplicationUser > userStore,
            RoleManager < IdentityRole > roleManager,
            ILogger<Log> logger,
            IMicroAgManagementDbContext dbContext, IEmailSender<ApplicationUser> emailSender, LinkGenerator linkGenerator) =>
            {
                try { 
                logger.LogDebug("In custom register");
                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException($" requires a user store with email support.");
                }
                
                var newUser = Guid.NewGuid();
                var newTenant = newUser;
                ApplicationUser user = new()
                {
                    Id = newUser.ToString().ToUpperInvariant(),
                    Email = registration.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registration.Email,
                    TenantId = newTenant,
                };

                await userStore.SetUserNameAsync(user, registration.Email, CancellationToken.None);
                var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
                await emailStore.SetEmailAsync(user, registration.Email, CancellationToken.None);

                var result = await userManager.CreateAsync(user, registration.Password);

                if (!result.Succeeded)
                    return CreateValidationProblem(result);

                if (!await roleManager.RoleExistsAsync("TenantAdmin"))
                    await roleManager.CreateAsync(new IdentityRole("TenantAdmin"));
                if (newTenant == newUser)
                {
                    var tenant = new Tenant(newTenant) { GuidId = newTenant, Name = registration.Email, TenantUserAdminId = newTenant };
                    dbContext.Tenants.Add(tenant);
                    await userManager.AddToRoleAsync(user, "TenantAdmin");
                }
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var userId = await userManager.GetUserIdAsync(user);
                var routeValues = new RouteValueDictionary()
                {
                    ["userId"] = userId,
                    ["code"] = code,
                };

                var confirmEmailUrl = linkGenerator.GetUriByName(context, "MapIdentityApi-/account/confirmEmail", routeValues);
                await emailSender.SendConfirmationLinkAsync(user, registration.Email, HtmlEncoder.Default.Encode(confirmEmailUrl));
                }
                catch(Exception ex) { logger.LogError($"{ex.Message} {ex.InnerException}"); }
                return TypedResults.Ok();
            });
        }
        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            // We expect a single error code and description in the normal case.
            // This could be golfed with GroupBy and ToDictionary, but perf! :P
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }
        private async static Task<IResult> ProcessQuery(BaseQuery query, IMediator mediator, HttpRequest request)
        {
            if (!(query is BaseQuery))
                return Results.BadRequest();
            try
            {
                query.TenantId = Guid.Parse(request.HttpContext.User.FindFirst("TenantId")?.Value);
                return Results.Ok(await mediator.Send(query));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        }
        private async static Task<IResult> ProcessCommand(BaseCommand command, IMediator mediator, HttpRequest request)
        {
            if (!(command is BaseCommand))
                return Results.BadRequest();
            try
            {
                command.TenantId = Guid.Parse(request.HttpContext.User.FindFirst("TenantId")?.Value);
                command.ModifiedBy = Guid.Parse(request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return Results.Ok(await mediator.Send(command));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        }
        public static void MapTest(WebApplication app)
        {
            app.MapGet("/api/Test", () => "Hello World Test!").RequireAuthorization();
            
        }

        public static void MapAnciliary(WebApplication app)
        {
            app.MapPost("/api/GetRegistrars", async (GetRegistrarList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateRegistrar", async (CreateRegistrar command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateRegistrar", async (UpdateRegistrar command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();

            app.MapPost("/api/GetRegistrations", async (GetRegistrationList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateRegistration", async (CreateRegistration command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateRegistration", async (UpdateRegistration command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();


            app.MapPost("/api/GetUnits", async (GetUnitList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateUnit", async (CreateUnit command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateUnit", async (UpdateUnit command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();

            app.MapPost("/api/GetMeasures", async (GetMeasureList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateMeasure", async (CreateMeasure command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateMeasure", async (UpdateMeasure command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();

            app.MapPost("/api/GetMeasurements", async (GetMeasurementList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateMeasurement", async (CreateMeasurement command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateMeasurement", async (UpdateMeasurement command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();


            app.MapPost("/api/GetTreatments", async (GetTreatmentList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateTreatment", async (CreateTreatment command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateTreatment", async (UpdateTreatment command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();

            app.MapPost("/api/GetTreatmentRecords", async (GetTreatmentRecordList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateTreatmentRecord", async (CreateTreatmentRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateTreatmentRecord", async (UpdateTreatmentRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();


        }
        public static void MapFarm(WebApplication app)
        {
            app.MapPost("/api/GetTenants", async (GetTenantList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetFarm", async (GetFarm query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetFarms", async (GetFarmList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLandPlot", async (GetLandPlot query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLandPlots", async (GetLandPlotList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();

            app.MapPost("/api/CreateFarmLocation", async (CreateFarmLocation command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateFarmLocation", async (UpdateFarmLocation command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateLandPlot", async (CreateLandPlot command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateLandPlot", async (UpdateLandPlot command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
        }
        public static void MapJoins(WebApplication app)
        {
            app.MapPost("/api/GetDutyMilestoneList", async (GetDutyMilestoneList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetDutyEventList", async (GetDutyEventList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetDutyChoreList", async (GetDutyChoreList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
        }
        public static void MapLivestock(WebApplication app)
        {
            app.MapPost("/api/GetLivestockBreeds", async (GetLivestockBreedList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockAnimals", async (GetLivestockAnimalList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockStatuses", async (GetLivestockStatusList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestocks", async (GetLivestockList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockFeedAnalyses", async (GetLivestockFeedAnalysisList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockFeedAnalysisParameters", async (GetLivestockFeedAnalysisParameterList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockFeedAnalysisResults", async (GetLivestockFeedAnalysisResultList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockFeedDistributions", async (GetLivestockFeedDistributionList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockFeeds", async (GetLivestockFeedList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetLivestockFeedServings", async (GetLivestockFeedServingList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetBreedingRecords", async (GetBreedingRecordList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetBreedingRecord", async (GetBreedingRecord query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();

            app.MapPost("/api/CreateLivestockAnimal", async (CreateLivestockAnimal command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateLivestockAnimal", async (UpdateLivestockAnimal command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateLivestockFeed", async (CreateLivestockFeed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateLivestockFeed", async (UpdateLivestockFeed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateLivestockStatus", async (CreateLivestockStatus command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateLivestockStatus", async (UpdateLivestockStatus command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateLivestockBreed", async (CreateLivestockBreed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateLivestockBreed", async (UpdateLivestockBreed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateLivestock", async (CreateLivestock command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateLivestock", async (UpdateLivestock command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/ServiceLivestock", async (ServiceLivestock command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateBreedingRecord", async (CreateBreedingRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateBreedingRecord", async (UpdateBreedingRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
        }
        public static void MapScheduling(WebApplication app)
        {
            app.MapPost("/api/GetDuties", async (GetDutyList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetDuty", async (GetDuty query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetScheduledDuties", async (GetScheduledDutyList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetScheduledDuty", async (GetScheduledDuty query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetMilestones", async (GetMilestoneList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetMilestone", async (GetMilestone query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetEvents", async (GetEventList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetEvent", async (GetEvent query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetChores", async (GetChoreList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();
            app.MapPost("/api/GetChore", async (GetChore query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request)).RequireAuthorization();


            app.MapPost("/api/CreateDuty", async (CreateDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateDuty", async (UpdateDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateScheduledDuty", async (CreateScheduledDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateScheduledDuty", async (UpdateScheduledDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateMilestone", async (CreateMilestone command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateMilestone", async (UpdateMilestone command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateEvent", async (CreateEvent command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateEvent", async (UpdateEvent command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateChore", async (CreateChore command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateChore", async (UpdateChore command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
        }

    }
}
