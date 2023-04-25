using Server.Models.Authentication;
using MediatR;
using Server.Abstracts;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Domain.Entity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Server.BusinessLogic.Authentication
{
    public class RefreshTokenCommand:IRequest<TokenModel>
    {
        [Required(ErrorMessage = "Token Is Required")]
        public TokenModel? Token { get; set; }
        public class Handler : AuthenticationCommandHandler, IRequestHandler<RefreshTokenCommand, TokenModel>
        {
            public Handler(IMicroAgManagementDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMediator mediator) : base(context, userManager, configuration, mediator)
            {
            }

            async public Task<TokenModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                if (request.Token is null)
                    return new TokenModel();
                string? accessToken = request.Token.jwtBearer;
                string? refreshToken = request.Token.refreshToken;
                var principal = AuthenticationHelpers.GetPrincipalFromExpiredToken(accessToken, _configuration);
                if (principal == null)
                    return new TokenModel();
                string username = principal.Identity?.Name ?? string.Empty;
                var user = await _userManager.FindByNameAsync(username);
                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                    return new TokenModel();

                var newAccessToken = await AuthenticationHelpers.CreateModelToken(user, _userManager, _configuration);
                user.RefreshToken = newAccessToken.refreshToken;
                await _userManager.UpdateAsync(user);

                return newAccessToken;
            }
        }
    }
}
