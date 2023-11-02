using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Measurement
{
    public class MeasurementQueries : BaseQuery
    {
        public MeasurementModel? NewMeasurement { get => (MeasurementModel?)NewModel; set => NewModel = value; }
        public long? MeasureId { get; set; }
        public long? RecipientTypeId { get; set; }
        public string? RecipientType { get; set; }
        public long? RecipientId { get; set; }
        public long? MeasurementUnitId { get; set; }
        public DateTime? DatePerformed { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Measurements.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (MeasureId != null) query = query.Where(_ => _.MeasureId == MeasureId);
            if (RecipientTypeId != null) query = query.Where(_ => _.RecipientTypeId == RecipientTypeId);
            if (RecipientType != null) query = query.Where(_ => _.RecipientType == RecipientType);
            if (RecipientId != null) query = query.Where(_ => _.RecipientId == RecipientId);
            if (MeasurementUnitId != null) query = query.Where(_ => _.MeasurementUnitId == MeasurementUnitId);
            if (DatePerformed != null) query = query.Where(_ => _.DatePerformed == DatePerformed);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;


        }
    }
}
