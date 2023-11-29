using Domain.Entity;
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
        [ForeignKey(nameof(Farm))]
        public long FarmLocationId { get; set; }
        public virtual FarmLocationModel Farm { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Precision(18, 3)] public decimal Area { get; set; } = 0M;
        [Required]
        [ForeignKey(nameof(AreaUnit))] public long AreaUnitId { get; set; }
        public virtual UnitModel AreaUnit { get; set; }
        
        public string Usage { get; set; } = nameof(LandPlotUseConstants.GeneralUse);
        [ForeignKey(nameof(LandPlotModel))] public long? ParentPlotId { get; set; }
        public virtual ICollection<LandPlotModel> Subplots { get; set; }=new List<LandPlotModel>();
        public virtual ICollection<LivestockModel> Livestocks { get; set; } = new List<LivestockModel>();

        public static LandPlotModel? Create(LandPlot plot)
        {
            var model = PopulateBaseModel(plot, new LandPlotModel
            {
                FarmLocationId = plot.FarmLocationId,
                Area=plot.Area,
                AreaUnitId = plot.AreaUnitId,
                Usage = plot.Usage,
                ParentPlotId = plot.ParentPlotId,
                Name= plot.Name,
                Description= plot.Description,
                Subplots=plot.Subplots?.Select(Create).ToList() ?? new List<LandPlotModel>(),
                Livestocks=plot.Livestocks?.Select(LivestockModel.Create).ToList() ?? new List<LivestockModel>()
            }) as LandPlotModel;
            return model;
        }


        public override BaseModel Map(BaseModel entity)
        {
            if (entity is not LandPlotModel || entity is null) return null;
            ((LandPlotModel)entity).Name = Name ?? string.Empty;
            ((LandPlotModel)entity).ParentPlotId = ParentPlotId;
            ((LandPlotModel)entity).Area = Area;
            ((LandPlotModel)entity).Usage = Usage;
            ((LandPlotModel)entity).AreaUnitId = AreaUnitId;
            ((LandPlotModel)entity).Description = Description ?? string.Empty;
            ((LandPlotModel)entity).FarmLocationId = FarmLocationId;
            if (((LandPlotModel)entity).Subplots?.Any() ?? false)
                foreach (var subplot in ((LandPlotModel)entity).Subplots)
                    Subplots.FirstOrDefault(p => p?.Id == subplot.Id)?.Map(subplot);
            if (((LandPlotModel)entity).Livestocks?.Any() ?? false)
                foreach (var livestock in ((LandPlotModel)entity).Livestocks)
                    Livestocks.FirstOrDefault(p => p?.Id == livestock.Id)?.Map(livestock);
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity is not LandPlot || entity is null) return null;
            ((LandPlot)entity).Name = Name ?? string.Empty;
            ((LandPlot)entity).ParentPlotId = ParentPlotId;
            ((LandPlot)entity).Area = Area;
            ((LandPlot)entity).Usage = Usage;
            ((LandPlot)entity).AreaUnitId = AreaUnitId;
            ((LandPlot)entity).Description = Description ?? string.Empty;
            ((LandPlot)entity).FarmLocationId = FarmLocationId;
            if (((LandPlot)entity).Subplots?.Any() ?? false)
                foreach (var subplot in ((LandPlot)entity).Subplots)
                    Subplots.FirstOrDefault(p => p?.Id == subplot.Id)?.Map(subplot);
            if (((LandPlot)entity).Livestocks?.Any() ?? false)
                foreach (var livestock in ((LandPlot)entity).Livestocks)
                    Livestocks.FirstOrDefault(p => p?.Id == livestock.Id)?.Map(livestock);
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
