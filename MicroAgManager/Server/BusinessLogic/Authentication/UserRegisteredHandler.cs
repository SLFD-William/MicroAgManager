using Domain.Interfaces;
using MediatR;

namespace Server.BusinessLogic.Authentication
{
    public class UserRegisteredHandler : INotificationHandler<UserRegistered>
    {
        protected readonly IMediator _mediator;
        protected readonly IMicroAgManagementDbContext _context;

        public UserRegisteredHandler(IMediator mediator, IMicroAgManagementDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
        {
            var createdTenant = _context.Tenants.Find(notification.User.TenantId);
            if (createdTenant is null)
                throw new Exception($"Tenant {notification.User.TenantId} does not exist");

            //if (_context.Farms.Any(p => p.TenantId == createdTenant.Id))
            //    return ;
            //var farm = new Domain.Entity.FarmLocation(Guid.Parse(notification.User.Id), createdTenant.Id)
            //{
            //    Name=createdTenant.Name
            //};

            //_context.Farms.Add(farm);
            //await _context.SaveChangesAsync(cancellationToken);
            return ;
            //if (_mediator != null)
            //await _mediator.Publish("New User Registration Complete",cancellationToken);
        }
       
    }
}
