using Domain.Abstracts;
using Domain.Constants;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class LandPlot : BaseEntity
    {
        public LandPlot(Guid createdBy,Guid tenantId):base(createdBy, tenantId)
        { }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Area { get; set; } = 0M;
        public string AreaUnit { get; set; }=UnitEnum.Area_Acres.GetDescription();
        public string Usage { get; set; } = LandPlotUseConstants.GeneralUse;
        [Required] public long FarmLocationId { get; set; }
        public long? ParentPlotId { get; set; }
        public virtual ICollection<LandPlot>? Subplots { get; set; }
        public virtual FarmLocation FarmLocation { get; set; }
        public virtual LandPlot? ParentPlot { get; set; }
        public ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();

    }
}
