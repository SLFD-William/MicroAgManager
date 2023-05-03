﻿using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    //Create a class called UpdateLivestockFeed implementing BaseCommand and IUpdateCommand
    public class UpdateLivestockFeed : BaseCommand, IUpdateCommand
    {
        //Add a property called ModifiedBy of type Guid
        public Guid ModifiedBy { get; set; }
        //Add a property called TenantId of type Guid
        public Guid TenantId { get; set; }
        //Add a property called LivestockFeed of type LivestockFeedModel
        public Domain.Models.LivestockFeedModel LivestockFeed { get; set; }
        //create a handler class implementing BaseCommandHandler<UpdateLivestockFeed>
        public class Handler : BaseCommandHandler<UpdateLivestockFeed>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateLivestockFeed request, CancellationToken cancellationToken)
            {
                var livestockFeed = _context.LivestockFeeds.Find(request.LivestockFeed.Id);
                livestockFeed = request.LivestockFeed.MapToEntity(livestockFeed);
                livestockFeed.ModifiedOn = DateTime.Now;
                livestockFeed.ModifiedBy = request.ModifiedBy;
                livestockFeed.TenantId = request.TenantId;
                _context.LivestockFeeds.Update(livestockFeed);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockFeed.Id.ToString(), livestockFeed.GetType().Name, "Modified", livestockFeed.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockFeed.Id;

            }
        }
    }
}