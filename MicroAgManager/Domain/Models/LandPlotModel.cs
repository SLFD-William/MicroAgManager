using Domain.Entity;
using Domain.Enums;
using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class LandPlotModel : BaseModel
    {
        [Required]
        [ForeignKey(nameof(FarmLocationModel))]
        public long FarmLocationId { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public string? Description { get; set; }
        [Precision(18, 3)] public decimal Area { get; set; } = 0M;
        public long AreaUnit { get; set; } = UnitEnum.Area_Acres.Id;
        public string Usage { get; set; } = LandPlotUseConstants.GeneralUse;
        public long? ParentPlotId { get; set; }
        public virtual ICollection<LandPlotModel?> Subplots { get; set; }=new List<LandPlotModel?>();

        public static LandPlotModel? Create(LandPlot plot)
        {
            var model = PopulateBaseModel(plot, new LandPlotModel
            {
                FarmLocationId = plot.FarmLocationId,
                Area=plot.Area,
                AreaUnit = plot.AreaUnit.Id,
                Usage = plot.Usage,
                ParentPlotId = plot.ParentPlotId,
                Name= plot.Name,
                Description= plot.Description,
                Subplots=plot.Subplots?.Select(Create).ToList() ?? new List<LandPlotModel?>()
            }) as LandPlotModel;
            return model;
        }

        public LandPlot MapToEntity(LandPlot entity)
        {
            entity.Name = Name ?? string.Empty;
            entity.ParentPlotId = ParentPlotId;
            entity.Area = Area;
            entity.Usage =Usage;
            entity.AreaUnit = BaseEnumeration.FromId<UnitEnum>(AreaUnit);
            entity.Description = Description ?? string.Empty;
            entity.FarmLocationId = FarmLocationId;
            if(entity.Subplots?.Any() ?? false)
                foreach(var subplot in entity.Subplots)
                    Subplots.FirstOrDefault(p => p?.Id == subplot.Id)?.MapToEntity(subplot);

            return entity;
        }
    }
}
