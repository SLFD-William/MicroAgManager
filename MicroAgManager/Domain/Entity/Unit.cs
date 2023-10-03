using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
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
