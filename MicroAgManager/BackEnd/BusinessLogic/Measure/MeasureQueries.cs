using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Measure
{
    public class MeasureQueries : BaseQuery
    {
        public MeasureModel? NewMeasure { get => (MeasureModel?)NewModel; set => NewModel = value; }
        public string? Method { get; set; }
        public string? Name { get; set; }
        public long? UnitId { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Measures.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (Method != null) query = query.Where(_ => _.Method == Method);
            if (Name != null) query = query.Where(_ => _.Name == Name);
            if (UnitId != null) query = query.Where(_ => _.UnitId == UnitId);
            return (IQueryable<T>)query;


        }
    }
}