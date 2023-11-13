using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Unit : BaseEntity
    {
        public Unit(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(20)] public string Name { get; set; }
        [Required][MaxLength(20)] public string Category { get; set; }
        [Required][MaxLength(20)] public string Symbol { get; set; }
        [Required] required public double ConversionFactorToSIUnit { get; set; } = 1;
    }
}
