using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock
{
    public class CreateLivestock:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockModel Livestock { get; set; }
        public string CreationMode { get; set; } = "Create";
        public class Handler : BaseCommandHandler<CreateLivestock>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestock request, CancellationToken cancellationToken)
            {
                var livestock = new Domain.Entity.Livestock(request.ModifiedBy, request.TenantId);
                livestock = request.Livestock.Map(livestock) as Domain.Entity.Livestock;
                _context.Livestocks.Add(livestock);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    if(request.CreationMode=="Birth")
                        await _mediator.Publish(new LivestockBorn { EntityName = livestock.GetType().Name, Id = livestock.Id, ModifiedBy = request.ModifiedBy, TenantId = request.TenantId }, cancellationToken);
                    else
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Created", livestock.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestock.Id;
            }
        }

    }
}
