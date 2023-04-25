using Server.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Server.Models.Authentication;

namespace Server.BusinessLogic.Authentication
{
    public class LoginUserCommand : AuthenticationCommand
    {
        public class Handler : AuthenticationCommandHandler, IRequestHandler<LoginUserCommand, LoginResult>
        {
            private readonly SignInManager<ApplicationUser> _signInManager;
            public Handler(IMicroAgManagementDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMediator mediator, SignInManager<ApplicationUser> signInManager) : base(context, userManager, configuration, mediator)
            {
                _signInManager = signInManager;
            }

            public async Task<LoginResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                var loginResult = new LoginResult { success = false };
                var user = await _userManager.FindByNameAsync(request.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, true);
                    loginResult.success = result.Succeeded;
                    if (!result.Succeeded)
                        loginResult.message = AuthenticationConstants.LoginInvalid;

                    if (result.IsLockedOut)
                        loginResult.message = AuthenticationConstants.LoginLocked;

                    if (result.IsNotAllowed)
                        loginResult.message = AuthenticationConstants.LoginUnauthorized;

                    if (!loginResult.success)
                        return loginResult;

                    var token = await AuthenticationHelpers.CreateModelToken(user, _userManager, _configuration);
                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = token.refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    await _userManager.UpdateAsync(user);
                    loginResult.message = string.Empty;
                    loginResult.success = true;
                    loginResult.token = token;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return loginResult;
            }
        }
    }
}
