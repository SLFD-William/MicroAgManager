using MediatR;
using BackEnd.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BackEnd.BusinessLogic.Tenant;
using BackEnd.BusinessLogic.FarmLocation;
using Microsoft.AspNetCore.Authorization;
using BackEnd.BusinessLogic.LandPlots;
using BackEnd.BusinessLogic.Livestock;
using BackEnd.BusinessLogic.LivestockFeed;

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
        public static void Map(WebApplication app)
        {
            app.MapPost("/api/GetTenants",[Authorize] async (GetTenantList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetFarm", [Authorize] async (GetFarm query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetFarms", [Authorize] async (GetFarmList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLandPlot", [Authorize] async (GetLandPlot query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLandPlots", [Authorize] async (GetLandPlotList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockBreeds", [Authorize] async (GetLivestockBreedList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockTypes", [Authorize] async (GetLivestockTypeList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockStatuses", [Authorize] async (GetLivestockStatusList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestocks", [Authorize] async (GetLivestockList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedAnalyses", [Authorize] async (GetLivestockFeedAnalysisList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedAnalysisParameters", [Authorize] async (GetLivestockFeedAnalysisParameterList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedAnalysisResults", [Authorize] async (GetLivestockFeedAnalysisResultList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedDistributions", [Authorize] async (GetLivestockFeedDistributionList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeeds", [Authorize] async (GetLivestockFeedList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/GetLivestockFeedServings", [Authorize] async (GetLivestockFeedServingList query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));




            app.MapPost("/api/CreateFarmLocation", [Authorize] async (CreateFarmLocation command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateFarmLocation", [Authorize] async (UpdateFarmLocation command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/CreateLandPlot", [Authorize] async (CreateLandPlot command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
            app.MapPost("/api/UpdateLandPlot", [Authorize] async (UpdateLandPlot command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));


        }
    }
}
