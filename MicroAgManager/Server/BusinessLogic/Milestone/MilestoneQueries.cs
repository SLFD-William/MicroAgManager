using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Milestone
{
    public class MilestoneQueries:BaseQuery
    {
        public MilestoneModel? NewDuty { get => (MilestoneModel?)NewModel; set => NewModel = value; }
        public string? Subcategory { get; set; }
        public bool? SystemRequired { get; set; }
        

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Milestones.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (Subcategory != null) query = query.Where(_ => _.Subcategory == Subcategory);
            if (SystemRequired != null) query = query.Where(_ => _.SystemRequired == SystemRequired);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }
    }
}
