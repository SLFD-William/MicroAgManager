using Domain.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MicroAgManager
{
    public class ApiClaimsTransformation : IClaimsTransformation
    {
        private UserManager<ApplicationUser> _userManager;
        private static readonly string claimTenantId = "TenantId";
        public ApiClaimsTransformation(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            if (!principal.HasClaim(claim => claim.Type == claimTenantId))
            {
                var user = await _userManager.GetUserAsync(principal);
                claimsIdentity.AddClaim(new Claim(claimTenantId, user?.TenantId.ToString() ?? string.Empty));
            }

            principal.AddIdentity(claimsIdentity);
            return principal;
        }
    }
}
