using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class CreateLivestockFeed : BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockFeedModel LivestockFeed { get; set; }

        //create a handler class implementing BaseCommandHandler<CreateLivestockFeed>
        public class Handler : BaseCommandHandler<CreateLivestockFeed>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(CreateLivestockFeed request, CancellationToken cancellationToken)
            {
                var livestockFeed = new Domain.Entity.LivestockFeed(request.ModifiedBy, request.TenantId);
                livestockFeed = request.LivestockFeed.MapToEntity(livestockFeed);
                livestockFeed.ModifiedOn = livestockFeed.CreatedOn = DateTime.Now;
                livestockFeed.ModifiedBy = livestockFeed.CreatedBy = request.ModifiedBy;
                livestockFeed.TenantId = request.TenantId;
                _context.LivestockFeeds.Add(livestockFeed);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockFeed.Id;
            }
        }



    }
}
