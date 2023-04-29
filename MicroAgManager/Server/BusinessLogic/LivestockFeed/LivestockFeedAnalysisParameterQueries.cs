using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    public class LivestockFeedAnalysisParameterQueries : BaseQuery
    {
        public LivestockFeedAnalysisParameterModel? NewLivestockFeedAnalysisParameter { get => (LivestockFeedAnalysisParameterModel?)NewModel; set => NewModel = value; }
        public string? Parameter { get; set; }
        public string? Unit { get; set; }
        public string? Method { get; set; }
        public int? ReportOrder { get; set; }
        public string? SubParameter { get; set; }

        public IQueryable<Domain.Entity.LivestockFeedAnalysisParameter> GetQuery(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockFeedAnalysisParameters.AsQueryable());
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (!string.IsNullOrWhiteSpace(Parameter)) query = query.Where(x => x.Parameter == Parameter);
            if (!string.IsNullOrWhiteSpace(Unit)) query = query.Where(x => x.Unit == Unit);
            if (!string.IsNullOrWhiteSpace(Method)) query = query.Where(x => x.Method == Method);
            if (ReportOrder.HasValue) query = query.Where(x => x.ReportOrder == ReportOrder);
            if (!string.IsNullOrWhiteSpace(SubParameter)) query = query.Where(x => x.SubParameter == SubParameter);

            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }

    }
}
