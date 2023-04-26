using Domain.Models;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Onboarding
{
    public partial class Welcome
    {

        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        protected TenantModel? tenant { get; private set; }
        protected override void OnInitialized()
        {
            if (dbContext is null) return;
            tenant = dbContext.Tenants.FirstOrDefault();
        }

    }
}
