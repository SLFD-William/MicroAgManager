using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class UpdateFarmLocation:BaseCommand, IRequest<long>,IUpdateCommand
    {
        public FarmLocationModel Farm { get => (FarmLocationModel)Model; set => Model = value; }

        public class Handler : BaseCommandHandler
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
        }

    }
}
