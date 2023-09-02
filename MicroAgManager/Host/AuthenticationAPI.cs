using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;
using Domain.ValueObjects;
using BackEnd.Authentication;
using BackEnd.Infrastructure;

namespace Host
{
    public static class AuthenticationAPI
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/api/security/register",
                [AllowAnonymous] async (RegisterUserCommand command, IMediator mediator, IConfiguration configuration, UserManager<ApplicationUser> userManager,IMicroAgManagementDbContext context) =>
                {
                    if (!(command is AuthenticationCommand)) return Results.BadRequest();
                    try
                    {

                        var userExists = await userManager.FindByNameAsync(command.Email);
                        if (userExists != null)
                            return Results.Ok(new LoginResult { success = false, message = AuthenticationConstants.RegistrationUserExists });

                        var newUser = Guid.NewGuid();
                        var newTenant= command.TenantId.HasValue ? command.TenantId.Value: newUser;

                        ApplicationUser user = new()
                        {
                            Id = newUser.ToString().ToUpperInvariant(),
                            Email = command.Email,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            UserName = command.Email,
                            TenantId = newTenant,
                            EmailConfirmed = true //TODO Get Email Confirmation Working.
                        };

                        var result = await userManager.CreateAsync(user, command.Password);
                        if (!result.Succeeded)
                            return Results.Unauthorized();
                        if (!command.TenantId.HasValue)
                        {
                            var tenant = new Tenant(newTenant) { GuidId = newTenant, Name=command.Email, TenantUserAdminId = newTenant };
                            context.Tenants.Add(tenant);
                            await userManager.AddToRoleAsync(user, "TenantAdmin");
                        }
                        
                        await context.SaveChangesAsync(new());
                        var tenantState= command.TenantId.HasValue ?"Updated":"Created";
                        await mediator.Publish(new EntitiesModifiedNotification(user.TenantId, new() 
                            { new ModifiedEntity(user.TenantId.ToString(), nameof(Tenant),  command.TenantId.HasValue ? "Updated" : "Created", Guid.Parse(user.Id)) }), new());
                        var loginResult = new LoginResult { success = true };

                        var token = await AuthenticationHelpers.CreateModelToken(user, userManager, configuration);
                        _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                        user.RefreshToken = token.refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                        await userManager.UpdateAsync(user);
                        loginResult.message = string.Empty;
                        loginResult.success = true;
                        loginResult.token = token;

                        if (loginResult.success)
                            return Results.Ok(loginResult);
                        if (loginResult.message == AuthenticationConstants.RegistrationFailed)
                            return Results.Unauthorized();
                        if (loginResult.message == AuthenticationConstants.RegistrationUserExists)
                            return Results.Ok(loginResult);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex);
                    }
                    return Results.BadRequest();
                }
            );
            app.MapPost("/api/security/login",
                [AllowAnonymous] async (LoginUserCommand command, IConfiguration configuration, SignInManager <ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) =>
                {
                    if (!(command is IRequest<LoginResult>))
                        return Results.BadRequest();

                    var loginResult = new LoginResult { success = false };
                    var user = await userManager.FindByNameAsync(command.Email);
                    if (user != null && await userManager.CheckPasswordAsync(user, command.Password))
                    {
                        var result = await signInManager.PasswordSignInAsync(command.Email, command.Password, command.RememberMe, true);
                        loginResult.success = result.Succeeded;
                        if (!result.Succeeded)
                            loginResult.message = AuthenticationConstants.LoginInvalid;

                        if (result.IsLockedOut)
                            loginResult.message = AuthenticationConstants.LoginLocked;

                        if (result.IsNotAllowed)
                            loginResult.message = AuthenticationConstants.LoginUnauthorized;


                        var token = await AuthenticationHelpers.CreateModelToken(user, userManager, configuration);
                        _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                        user.RefreshToken = token.refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                        await userManager.UpdateAsync(user);
                        loginResult.message = string.Empty;
                        loginResult.success = true;
                        loginResult.token = token;
                    }


                    if (loginResult.success)
                        return Results.Ok(loginResult);
                    if (loginResult.message == AuthenticationConstants.LoginUnauthorized || loginResult.message == AuthenticationConstants.LoginLocked)
                        return Results.BadRequest(loginResult);

                    return Results.Unauthorized();
                }
            );
            app.MapPost("/api/security/refreshtoken",
                [AllowAnonymous] async (RefreshTokenCommand command, IMicroAgManagementDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration) =>
                {
                    if (!(command is IRequest<TokenModel>))
                        return Results.BadRequest("Invalid access token or refresh token");

                    if (command.Token is null)
                        return Results.BadRequest("Invalid access token or refresh token");
                    string? accessToken = command.Token.jwtBearer;
                    string? refreshToken = command.Token.refreshToken;
                    var principal = AuthenticationHelpers.GetPrincipalFromExpiredToken(accessToken, configuration);
                    if (principal == null)
                        return Results.BadRequest("Invalid access token or refresh token");
                    string username = principal.Identity?.Name ?? string.Empty;
                    var user = await userManager.FindByNameAsync(username);
                    if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                        return Results.BadRequest("Invalid access token or refresh token");

                    var newAccessToken = await AuthenticationHelpers.CreateModelToken(user, userManager, configuration);
                    user.RefreshToken = newAccessToken.refreshToken;
                    await userManager.UpdateAsync(user);

                    if (newAccessToken is null)
                        return Results.BadRequest("Invalid access token or refresh token");

                    return Results.Ok(newAccessToken);
                }
            );
        }


    }
}
