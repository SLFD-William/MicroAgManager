using Domain.Entity;
using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class LandPlotModel : BaseModel,ILandPlot
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
        [ForeignKey(nameof(ParentPlot))] public long? ParentPlotId { get; set; }
        public virtual LandPlotModel? ParentPlot { get; set; }
        [NotMapped]public string ParentPlotName { get => ParentPlot?.Name ?? string.Empty; }
        public virtual ICollection<LandPlotModel> Subplots { get; set; }=new List<LandPlotModel>();
        public virtual ICollection<LivestockModel> Livestocks { get; set; } = new List<LivestockModel>();
        [NotMapped] DateTime ILandPlot.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value == EntityModifiedOn ? EntityModifiedOn : EntityModifiedOn; }
        IUnit? ILandPlot.AreaUnit { get => AreaUnit; set => AreaUnit =value as UnitModel ?? AreaUnit; }
         ICollection<ILivestock>? ILandPlot.Livestocks { get => Livestocks as ICollection<ILivestock>; set => Livestocks = value as ICollection<LivestockModel> ?? new List<LivestockModel>(); }
         ICollection<ILandPlot>? ILandPlot.Subplots { get => Subplots as ICollection<ILandPlot>; set => Subplots = value as ICollection<LandPlotModel> ?? new List<LandPlotModel>(); }

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
            ((LandPlotModel)entity).EntityModifiedOn = EntityModifiedOn;
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
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
