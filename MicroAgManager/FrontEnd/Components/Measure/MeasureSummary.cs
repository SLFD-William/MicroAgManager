using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.Measure
{
    public class MeasureSummary : ValueObject
    {
        private MeasureModel _measureModel;
        public MeasureSummary(MeasureModel measureModel, FrontEndDbContext context)
        {
            _measureModel = measureModel;
            Unit = context.Units.Find(measureModel.UnitId)?.Symbol ?? string.Empty;
        }
        public string Unit { get; private set; }
        public long Id => _measureModel.Id;
        public string Name => _measureModel.Name;
        public string Method => _measureModel.Method;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _measureModel;
        }
    }
}

