using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class LandPlotQueries : BaseQuery
    {
        public LandPlotModel? NewLandPlot { get => (LandPlotModel?)NewModel; set => NewModel = value; }
        public long? FarmLocationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Usage { get; set; }
        public long? ParentPlotId { get; set; }


        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Plots.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));

            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (FarmLocationId.HasValue) query = query.Where(_ => _.FarmLocationId == FarmLocationId);
            if (!string.IsNullOrEmpty(Description)) query = query.Where(_ => _.Description != null && _.Description.Contains(Description));
            if (ParentPlotId.HasValue) query = query.Where(_ => _.ParentPlotId == ParentPlotId);
            if (!string.IsNullOrEmpty(Usage)) query = query.Where(_ => _.Usage == Usage);

            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }
    }
}
