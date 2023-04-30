using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Abstracts
{
    public abstract class BaseNotificationHandler : INotificationHandler<BaseNotification>
    {
        protected readonly IMediator _mediator;
        protected readonly IMicroAgManagementDbContext _context;

        public BaseNotificationHandler(IMediator mediator, IMicroAgManagementDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public abstract Task Handle(BaseNotification notification, CancellationToken cancellationToken);
       
    }
}
