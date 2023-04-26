using Domain.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Abstracts
{
    public class BaseCommandHandler
    {
        protected readonly IMicroAgManagementDbContext _context;
        protected readonly IMediator _mediator;

        public BaseCommandHandler(IMicroAgManagementDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public virtual async Task<long> Handle(BaseCommand request, CancellationToken cancellationToken)
        {
            var entity = await HandleObject(request, cancellationToken);
            return (entity is BaseEntity) ? ((BaseEntity)entity).Id : ((BaseModel)entity).Id;
        }
        protected async Task<object> HandleObject(BaseCommand request, CancellationToken cancellationToken)
        {
            var message = new NotificationMessage { To = request.TenantId.ToString(), Body = $"Treatment {request.Model.Id}", From = request.ModifiedBy.ToString() };
            if (request is ICreateCommand)
                message.Subject = "Create";
            if (request is IUpdateCommand)
                message.Subject = "Update";
            if (request is IDeleteCommand || request is IHardDeleteCommand)
                message.Subject = "Delete";
            if (string.IsNullOrEmpty(message.Subject))
                throw new Exception("Unknown Command");
            var entity = await SaveObject(request, _context, cancellationToken);

            if (_context is IMicroAgManagementDbContext)
                message.Body = $"{entity.GetType().Name},{((BaseEntity)entity).Id}";

            if (_mediator is not null)
                await _mediator.Publish(message, cancellationToken);
            return entity;

        }
        static public async Task<object> SaveObject(BaseCommand request, IMicroAgManagementDbContext context, CancellationToken cancellationToken, bool submitData = true)
        {
            if (context is IMicroAgManagementDbContext)
                return await PersistEntity(request, context, cancellationToken, submitData);

            return null;
        }
        static public async Task<BaseEntity> PersistEntity(BaseCommand request, IMicroAgManagementDbContext context, CancellationToken cancellationToken, bool submitData = true)
        {
            var entity = GetNewEntity(request);

            Utilities.MapObjectToObject(entity, request.Model);
            entity.ModifiedBy = request.ModifiedBy;
            entity.TenantId = request.TenantId;
            if (request is IUpdateCommand || request is IDeleteCommand || request is IHardDeleteCommand)
            {
                var dbEntity = await ((DbContext)context).FindAsync(entity.GetType(), request.Model.Id) as BaseEntity;
                Utilities.MapObjectToObject(dbEntity, request.Model);
                dbEntity.ModifiedBy = request.ModifiedBy;
                dbEntity.TenantId = request.TenantId;
                dbEntity.ModifiedOn = DateTime.Now;
                if (request is IDeleteCommand)
                {
                    dbEntity.DeletedBy = request.ModifiedBy;
                    dbEntity.Deleted = dbEntity.ModifiedOn;
                }
                //if (request is IHardDeleteCommand)
                //    ((DbContext)context).Entry(dbEntity).State = EntityState.Deleted;

                //if ((request is IDeleteCommand || request is IHardDeleteCommand) && dbEntity is BaseDomainActionEntity)
                //    ScheduledDutyLogic.HardDeleteScheduledDuties(dbEntity as BaseDomainActionEntity, context);

                entity = dbEntity;
            }

            if (request is ICreateCommand)
            {
                entity.CreatedBy = request.ModifiedBy;
                await ((DbContext)context).AddAsync(entity, cancellationToken);
            }
            if (submitData)
                await ((DbContext)context).SaveChangesAsync(cancellationToken);
            return entity;
        }
        private static BaseEntity GetNewEntity(BaseCommand request)
        {
            if (request.Model is FarmLocationModel)
                return new Domain.Entity.FarmLocation(request.ModifiedBy, request.TenantId);
            
            throw new Exception("Unknown Domain Entity");
        }
    }
}
