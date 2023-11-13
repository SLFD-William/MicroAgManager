using Domain.Models;

namespace BackEnd.BusinessLogic.FarmLocation.LandPlots
{
    public class LandPlotDto
    {
        public LandPlotDto(long count, ICollection<LandPlotModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<LandPlotModel> Models { get; set; }
    }
}
