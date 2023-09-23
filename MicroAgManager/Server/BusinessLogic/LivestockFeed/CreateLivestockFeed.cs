using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockFeed request, CancellationToken cancellationToken)
            {
                var livestockFeed = new Domain.Entity.LivestockFeed(request.ModifiedBy, request.TenantId);
                livestockFeed = request.LivestockFeed.MapToEntity(livestockFeed);
                _context.LivestockFeeds.Add(livestockFeed);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Livestock Feed"); }
            
                return livestockFeed.Id;
            }
        }



    }
}
