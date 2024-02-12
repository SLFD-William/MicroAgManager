using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public interface IUnit
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Category { get; set; }
        double ConversionFactorToSIUnit { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Unit : BaseEntity, IUnit
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
