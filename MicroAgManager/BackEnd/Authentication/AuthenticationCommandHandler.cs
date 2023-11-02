using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BackEnd.Authentication
{
    public abstract class AuthenticationCommandHandler
    {
        protected readonly IMicroAgManagementDbContext _context;
        protected readonly IMediator _mediator;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IConfiguration _configuration;

        protected AuthenticationCommandHandler(IMicroAgManagementDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMediator mediator)//, IConfiguration configuration)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _configuration = configuration;
        }
    }
}
