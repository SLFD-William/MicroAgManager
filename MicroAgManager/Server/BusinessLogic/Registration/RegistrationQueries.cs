using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Registration
{
    public class RegistrationQueries : BaseQuery
    {

        public RegistrationModel NewRegistration { get => (RegistrationModel)NewModel; set => NewModel = value; }
        public string Registrar { get; set; }
        
        public long? RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        public long? RecipientId { get; set; }
        public string Identifier { get; set; }
        public bool? DefaultIdentification { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Registrations.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (!string.IsNullOrEmpty(Registrar)) query = query.Where(_ => _.Registrar != null && _.Registrar.Name.Contains(Registrar));
            if (RecipientTypeId.HasValue) query = query.Where(_ => _.RecipientTypeId == RecipientTypeId);
            if (!string.IsNullOrEmpty(RecipientType)) query = query.Where(_ => _.RecipientType != null && _.RecipientType.Contains(RecipientType));
            if (RecipientId.HasValue) query = query.Where(_ => _.RecipientId == RecipientId);
            if (!string.IsNullOrEmpty(Identifier)) query = query.Where(_ => _.Identifier != null && _.Identifier.Contains(Identifier));
            if (DefaultIdentification.HasValue) query = query.Where(_ => _.DefaultIdentification == DefaultIdentification);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
            
        }
    }
}
