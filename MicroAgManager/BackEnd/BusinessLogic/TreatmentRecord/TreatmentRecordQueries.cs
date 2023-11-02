using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.TreatmentRecord
{
    public class TreatmentRecordQueries : BaseQuery
    {
        public TreatmentRecordModel? NewTreatmentRecord { get => (TreatmentRecordModel?)NewModel; set => NewModel = value; }
        public string? TreatmentName { get; set; }
        public string? RecipientType { get; set; }
        public string? AppliedMethod { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
           var query = PopulateBaseQuery(context.TreatmentRecords.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (TreatmentName != null) query = query.Where(_ => _.Treatment.Name == TreatmentName);
            if (RecipientType != null) query = query.Where(_ => _.RecipientType == RecipientType);
            if (AppliedMethod != null) query = query.Where(_ => _.AppliedMethod == AppliedMethod);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }
    }
}
