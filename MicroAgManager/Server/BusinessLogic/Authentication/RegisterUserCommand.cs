using BackEnd.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using BackEnd.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Authentication
{
    public class RegisterUserCommand : LoginUserCommand
    {
        [Required(ErrorMessage = "Confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confimation does not match.")]
        public string? ConfirmPassword { get; set; }
        [Display(Description = "Farm Name")]
        [Required(ErrorMessage = "Farm Name is required.")]
        public string? Name { get; set; }
        new public class Handler : AuthenticationCommandHandler, IRequestHandler<RegisterUserCommand, LoginResult>
        {
            public Handler(IMicroAgManagementDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMediator mediator) : base(context, userManager, configuration, mediator)
            {

            }
            public async Task<LoginResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var userExists = await _userManager.FindByNameAsync(request.Email);
                if (userExists != null)
                    return new LoginResult { success = false, message = AuthenticationConstants.RegistrationUserExists };

                var newTenant = Guid.NewGuid();
                var tenant = new Tenant(newTenant) { Id=newTenant, Name = request.Name ?? string.Empty, TenantUserAdminId =newTenant};

                ApplicationUser user = new()
                {
                    Id= newTenant.ToString().ToUpperInvariant(),
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.Email,
                    Tenant = tenant,
                    EmailConfirmed=true //TODO Get Email Confirmation Working.
                };


                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    return new LoginResult { success = false, message = AuthenticationConstants.RegistrationFailed };
                
                await _userManager.AddToRoleAsync(user, "TenantAdmin");
                await _context.SaveChangesAsync(cancellationToken);

                var loginResult = new LoginResult { success = true };

                var token = await AuthenticationHelpers.CreateModelToken(user, _userManager, _configuration);
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                user.RefreshToken = token.refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);


                await _userManager.UpdateAsync(user);
                loginResult.message = string.Empty;
                loginResult.success = true;
                loginResult.token = token;

                return loginResult;
            }

        }


    }
    
}
