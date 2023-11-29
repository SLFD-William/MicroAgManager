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

namespace MicroAgManager
{
    public static class BusinessLogicAPI
    {
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

            app.MapPost("/api/CreateDuty", async (CreateDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateDuty", async (UpdateDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateScheduledDuty", async (CreateScheduledDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateScheduledDuty", async (UpdateScheduledDuty command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateMilestone", async (CreateMilestone command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateMilestone", async (UpdateMilestone command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/CreateEvent", async (CreateEvent command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
            app.MapPost("/api/UpdateEvent", async (UpdateEvent command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request)).RequireAuthorization();
        }

    }
}
