using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.LandPlot
{
    public class LandPlotSummary:ValueObject
    {
        private LandPlotModel _landPlotModel;
        public LandPlotSummary(LandPlotModel landPlotModel, FrontEndDbContext context)
        {
            _landPlotModel = landPlotModel;
            Area = $"{landPlotModel.Area} {context.Units.Find(landPlotModel.AreaUnitId).Symbol}";
        }
        public long Id => _landPlotModel.Id;
        public string Name => _landPlotModel.Name;
        public string Description => _landPlotModel.Description;
        public string Area { get; private set; }
        public string Usage => _landPlotModel.Usage;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _landPlotModel;
        }
    }
}
