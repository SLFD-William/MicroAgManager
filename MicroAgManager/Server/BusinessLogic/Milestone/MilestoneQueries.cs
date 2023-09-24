using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Milestone
{
    public class MilestoneQueries:BaseQuery
    {
        public MilestoneModel? NewDuty { get => (MilestoneModel?)NewModel; set => NewModel = value; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? SystemRequired { get; set; }
        

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Milestones.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (Name != null) query = query.Where(_ => _.Name == Name);
            if (Description != null) query = query.Where(_ => _.Description == Description);
            if (SystemRequired != null) query = query.Where(_ => _.SystemRequired == SystemRequired);
            
            return (IQueryable<T>)query;
        }
    }
}
