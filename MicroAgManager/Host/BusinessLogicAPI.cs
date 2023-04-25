using MediatR;
using Microsoft.AspNetCore.Authorization;
using BackEnd.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                return Results.Ok(await mediator.Send(command));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
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
            app.MapPost("/api/Query", [Authorize] async (BaseQuery query, IMediator mediator, HttpRequest request) => await ProcessQuery(query, mediator, request));
            app.MapPost("/api/Command", [Authorize] async (BaseCommand command, IMediator mediator, HttpRequest request) => await ProcessCommand(command, mediator, request));
        }
    }
}
