using BackEnd.Abstracts;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class LandPlotQueries : BaseQuery
    {
        public LandPlotModel? NewLandPlot { get => (LandPlotModel?)NewModel; set => NewModel = value; }
        public bool? GetDeleted { get; set; }
        public long? FarmLocationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PlotUseEnum? Usage { get; set; }
        public long? ParentPlotId { get; set; }

        public IQueryable<Domain.Entity.LandPlot> GetQuery(IMicroAgManagementDbContext context)
        {
            var query = context.Plots.Where(f => f.TenantId == TenantId).AsQueryable();
            if (query is null)
                throw new ArgumentNullException(nameof(query));
            if (GetDeleted.HasValue && !GetDeleted.Value)
                query = query.Where(_ => !_.Deleted.HasValue);
            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);
            if (Name != null)
                query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (FarmLocationId != null)
                query = query.Where(_ => _.FarmLocationId==FarmLocationId);
            if (Description != null)
                query = query.Where(_ => _.Description != null && _.Description.Contains(Description));
            if (ParentPlotId != null)
                query = query.Where(_ => _.ParentPlotId == ParentPlotId);
            if (Usage is not null)
                query = query.Where(_ => _.Usage == Usage);

            if (LastModified.HasValue)
                query = query.Where(_ => _.ModifiedOn >= LastModified);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }
    }
}
