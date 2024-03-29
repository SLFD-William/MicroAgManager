﻿using MediatR;
using BackEnd.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BackEnd.BusinessLogic.Tenant;
using BackEnd.BusinessLogic.FarmLocation;
using Microsoft.AspNetCore.Authorization;
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

namespace Host
{
    public static class BusinessLogicAPI
    {
        private async static Task<IResult> ProcessQuery(BaseQuery query, IMediator mediator, HttpRequest request)
        {
            if (!(query is BaseQuery))
                return Results.BadRequest();
            try
            {
                query.TenantId = TenantId(request);
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
                command.TenantId = TenantId(request);
                command.ModifiedBy = UserId(request);
                return Results.Ok(await mediator.Send(command));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        }
        private static Guid UserId(HttpRequest request)
        {
            var headerToken = ((string)request.Headers["Authorization"]).Replace("Bearer ", string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(headerToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt"));
            return Guid.Parse(user.Claims.First(x => x.Type == "sub").Value);
        }
        private static Guid TenantId(HttpRequest request)
        {
            var headerToken = ((string)request.Headers["Authorization"]).Replace("Bearer ", string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(headerToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt"));
            return Guid.Parse(user.Claims.First(x => x.Type == "TenantId").Value);
        }
        public static void MapAnciliary(WebApplication app)
        {
            app.MapPost("/api/GetRegistrars", [Authorize] async (GetRegistrarList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateRegistrar", [Authorize] async (CreateRegistrar command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateRegistrar", [Authorize] async (UpdateRegistrar command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));

            app.MapPost("/api/GetRegistrations", [Authorize] async (GetRegistrationList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateRegistration", [Authorize] async (CreateRegistration command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateRegistration", [Authorize] async (UpdateRegistration command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));


            app.MapPost("/api/GetUnits", [Authorize] async (GetUnitList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateUnit", [Authorize] async (CreateUnit command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateUnit", [Authorize] async (UpdateUnit command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));

            app.MapPost("/api/GetMeasures", [Authorize] async (GetMeasureList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateMeasure", [Authorize] async (CreateMeasure command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateMeasure", [Authorize] async (UpdateMeasure command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));

            app.MapPost("/api/GetMeasurements", [Authorize] async (GetMeasurementList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateMeasurement", [Authorize] async (CreateMeasurement command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateMeasurement", [Authorize] async (UpdateMeasurement command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));


            app.MapPost("/api/GetTreatments", [Authorize] async (GetTreatmentList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateTreatment", [Authorize] async (CreateTreatment command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateTreatment", [Authorize] async (UpdateTreatment command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));

            app.MapPost("/api/GetTreatmentRecords", [Authorize] async (GetTreatmentRecordList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/CreateTreatmentRecord", [Authorize] async (CreateTreatmentRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateTreatmentRecord", [Authorize] async (UpdateTreatmentRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));


        }
        public static void MapFarm(WebApplication app)
        {
            app.MapPost("/api/GetTenants", [Authorize] async (GetTenantList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetFarm", [Authorize] async (GetFarm query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetFarms", [Authorize] async (GetFarmList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLandPlot", [Authorize] async (GetLandPlot query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLandPlots", [Authorize] async (GetLandPlotList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));

            app.MapPost("/api/CreateFarmLocation", [Authorize] async (CreateFarmLocation command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateFarmLocation", [Authorize] async (UpdateFarmLocation command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateLandPlot", [Authorize] async (CreateLandPlot command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLandPlot", [Authorize] async (UpdateLandPlot command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
        }
        public static void MapJoins(WebApplication app)
        {
            app.MapPost("/api/GetDutyMilestoneList", [Authorize] async (GetDutyMilestoneList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetDutyEventList", [Authorize] async (GetDutyEventList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
        }
        public static void MapLivestock(WebApplication app)
        {
            app.MapPost("/api/GetLivestockBreeds", [Authorize] async (GetLivestockBreedList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockAnimals", [Authorize] async (GetLivestockAnimalList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockStatuses", [Authorize] async (GetLivestockStatusList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestocks", [Authorize] async (GetLivestockList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedAnalyses", [Authorize] async (GetLivestockFeedAnalysisList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedAnalysisParameters", [Authorize] async (GetLivestockFeedAnalysisParameterList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedAnalysisResults", [Authorize] async (GetLivestockFeedAnalysisResultList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedDistributions", [Authorize] async (GetLivestockFeedDistributionList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeeds", [Authorize] async (GetLivestockFeedList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedServings", [Authorize] async (GetLivestockFeedServingList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetBreedingRecords", [Authorize] async (GetBreedingRecordList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetBreedingRecord", [Authorize] async (GetBreedingRecord query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));

            app.MapPost("/api/CreateLivestockAnimal", [Authorize] async (CreateLivestockAnimal command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLivestockAnimal", [Authorize] async (UpdateLivestockAnimal command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateLivestockFeed", [Authorize] async (CreateLivestockFeed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLivestockFeed", [Authorize] async (UpdateLivestockFeed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateLivestockStatus", [Authorize] async (CreateLivestockStatus command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLivestockStatus", [Authorize] async (UpdateLivestockStatus command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateLivestockBreed", [Authorize] async (CreateLivestockBreed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLivestockBreed", [Authorize] async (UpdateLivestockBreed command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateLivestock", [Authorize] async (CreateLivestock command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLivestock", [Authorize] async (UpdateLivestock command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/ServiceLivestock", [Authorize] async (ServiceLivestock command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateBreedingRecord", [Authorize] async (CreateBreedingRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateBreedingRecord", [Authorize] async (UpdateBreedingRecord command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
        }
        public static void MapScheduling(WebApplication app)
        {
            app.MapPost("/api/GetDuties", [Authorize] async (GetDutyList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetDuty", [Authorize] async (GetDuty query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetScheduledDuties", [Authorize] async (GetScheduledDutyList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetScheduledDuty", [Authorize] async (GetScheduledDuty query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetMilestones", [Authorize] async (GetMilestoneList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetMilestone", [Authorize] async (GetMilestone query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetEvents", [Authorize] async (GetEventList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetEvent", [Authorize] async (GetEvent query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));

            app.MapPost("/api/CreateDuty", [Authorize] async (CreateDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateDuty", [Authorize] async (UpdateDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateScheduledDuty", [Authorize] async (CreateScheduledDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateScheduledDuty", [Authorize] async (UpdateScheduledDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateMilestone", [Authorize] async (CreateMilestone command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateMilestone", [Authorize] async (UpdateMilestone command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateEvent", [Authorize] async (CreateEvent command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateEvent", [Authorize] async (UpdateEvent command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
        }

    }
}
