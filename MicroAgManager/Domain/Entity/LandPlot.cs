﻿using Domain.Abstracts;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LandPlot : BaseEntity
    {
        public LandPlot(Guid createdBy,Guid tenantId):base(createdBy, tenantId)
        { }

        [Required][MaxLength(50)]public string Name { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        [Precision(18,3)]public decimal Area { get; set; } = 0M;
        [Required][ForeignKey(nameof(AreaUnit))] public long AreaUnitId { get; set; }
        public virtual Unit AreaUnit { get; set; }
        [Required][MaxLength(50)] public string Usage { get; set; } = LandPlotUseConstants.GeneralUse;
        [Required] public long FarmLocationId { get; set; }

        [ForeignKey("ParentPlot")]
        public long? ParentPlotId { get; set; }
        public virtual LandPlot? ParentPlot { get; set; }
        public virtual ICollection<LandPlot>? Subplots { get; set; }
        public virtual FarmLocation FarmLocation { get; set; }
        
        public virtual ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();

    }
}
