﻿using Domain.Abstracts;
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
        public UnitEnum AreaUnit { get; set; }=UnitEnum.Area_Acres;
        public string Usage { get; set; } = LandPlotUseConstants.GeneralUse;
        [Required] public long FarmLocationId { get; set; }
        public long? ParentPlotId { get; set; }
        public virtual ICollection<LandPlot>? Subplots { get; set; }
        public virtual FarmLocation? FarmLocation { get; set; }
        
    }
}
