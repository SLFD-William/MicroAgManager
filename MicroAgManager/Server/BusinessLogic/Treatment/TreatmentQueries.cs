using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Treatment
{
    public class TreatmentQueries : BaseQuery
    {
        public TreatmentModel? NewTreatment { get => (TreatmentModel?)NewModel; set => NewModel = value; }
        public string? Name { get; set; }
        public string? BrandName { get; set; }
        public string? Reason { get; set; }
        public string? LabelMethod { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Treatments.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (Name != null) query = query.Where(_ => _.Name == Name);
            if (BrandName != null) query = query.Where(_ => _.BrandName == BrandName);
            if (Reason != null) query = query.Where(_ => _.Reason == Reason);
            if (LabelMethod != null) query = query.Where(_ => _.LabelMethod == LabelMethod);
            return (IQueryable<T>)query;
            
        }
    }
}
