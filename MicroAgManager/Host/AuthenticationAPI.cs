using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using BackEnd.Abstracts;
using BackEnd.BusinessLogic.Authentication;
using BackEnd.Models.Authentication;

namespace Host
{
    public static class AuthenticationAPI
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/api/security/register",
                [AllowAnonymous] async (RegisterUserCommand command, IMediator mediator) =>
                {
                    if (!(command is AuthenticationCommand))
                        return Results.BadRequest();
                    try
                    {

                        var result = await mediator.Send(command);
                        if (result.success)
                            return Results.Ok(result);
                        if (result.message == AuthenticationConstants.RegistrationFailed)
                            return Results.Unauthorized();
                        if (result.message == AuthenticationConstants.RegistrationUserExists)
                            return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex);
                    }
                    return Results.BadRequest();
                }
            );
            app.MapPost("/api/security/login",
                [AllowAnonymous] async (LoginUserCommand command, IMediator mediator) =>
                {
                    if (!(command is IRequest<LoginResult>))
                        return Results.BadRequest();

                    var result = await mediator.Send(command);
                    if (result.success)
                        return Results.Ok(result);
                    if (result.message == AuthenticationConstants.LoginUnauthorized || result.message == AuthenticationConstants.LoginLocked)
                        return Results.BadRequest(result);

                    return Results.Unauthorized();
                }
            );
            app.MapPost("/api/security/refreshtoken",
                [AllowAnonymous] async (RefreshTokenCommand command, IMediator mediator) =>
                {
                    if (!(command is IRequest<TokenModel>))
                        return Results.BadRequest("Invalid access token or refresh token");

                    var result = await mediator.Send(command);
                    if (result is null)
                        return Results.BadRequest("Invalid access token or refresh token");

                    return Results.Ok(result);
                }
            );
        }
    }
}
