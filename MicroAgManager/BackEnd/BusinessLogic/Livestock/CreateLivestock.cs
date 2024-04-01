using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock
{
    public class CreateLivestock : BaseCommand, ICreateCommand, ICreateLivestock
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockModel Livestock { get; set; }
        public string CreationMode { get; set; } = "Create";
        
        public class Handler : BaseCommandHandler<CreateLivestock>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestock request, CancellationToken cancellationToken)
            {
                var livestock = new Domain.Entity.Livestock(request.ModifiedBy, request.TenantId);
                livestock = request.Livestock.Map(livestock) as Domain.Entity.Livestock;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Livestocks.Add(livestock);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        if (request.CreationMode == "Birth")
                        {
                            var modifiedNotice = await LivestockLogic.OnLivestockBorn(context, livestock.Id, cancellationToken);
                            foreach(var mod in modifiedNotice)
                                await _mediator.Publish(new ModifiedEntityPushNotification (mod.TenantId, mod.ModelJson, mod.ModelType), cancellationToken);
                        }
                        else
                            await _mediator.Publish(new ModifiedEntityPushNotification (livestock.TenantId, LivestockModel.Create(livestock).GetJsonString(), nameof(LivestockModel)), cancellationToken);

                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                return livestock.Id;
            }
        }

    }
}
