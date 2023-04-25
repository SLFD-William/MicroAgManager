using Domain.Entity;
using Domain.Enums;
using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LandPlotModel : BaseModel
    {
        [Required]
        public long FarmLocationId { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public string? Description { get; set; }
        public decimal Area { get; set; } = 0M;
        public UnitEnum AreaUnit { get; set; } = UnitEnum.Area_Acres;
        public PlotUseEnum Usage { get; set; } = PlotUseEnum.GeneralUse;
        public long? ParentPlotId { get; set; }

        public static LandPlotModel? Create(LandPlot plot)
        {
            var model = PopulateBaseModel(plot, new LandPlotModel
            {
                FarmLocationId = plot.FarmLocationId,
                Area=plot.Area,
                AreaUnit = plot.AreaUnit,
                Usage = plot.Usage,
                ParentPlotId = plot.ParentPlotId,
                Name= plot.Name,
                Description= plot.Description,
            }) as LandPlotModel;
            return model;
        }
    }
}
