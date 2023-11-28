using Domain.Entity;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace MicroAgManager.Components.Account
{
    // This is a server-side AuthenticationStateProvider that uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
    internal sealed class PersistingServerAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
    {
        private readonly PersistentComponentState state;
        private readonly IdentityOptions options;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly PersistingComponentStateSubscription subscription;

        private Task<AuthenticationState>? authenticationStateTask;

        public PersistingServerAuthenticationStateProvider(
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor, UserManager<ApplicationUser> userManager)
        {
            state = persistentComponentState;
            options = optionsAccessor.Value;
            _userManager = userManager;
            AuthenticationStateChanged += OnAuthenticationStateChanged;
            subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }

        private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            authenticationStateTask = task;
        }

        private async Task OnPersistingAsync()
        {
            if (authenticationStateTask is null)
            {
                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
            }

            var authenticationState = await authenticationStateTask;
            var principal = authenticationState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = principal.FindFirst(options.ClaimsIdentity.UserIdClaimType)?.Value;
                var email = principal.FindFirst(options.ClaimsIdentity.EmailClaimType)?.Value;
                var tenantId =(await _userManager.FindByIdAsync(userId))?.TenantId.ToString();

                if (userId != null && email != null && tenantId != null)
                {
                    state.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        UserId = userId,
                        Email = email,
                        TenantId = tenantId
                    });
                }
            }
        }

        public void Dispose()
        {
            subscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
