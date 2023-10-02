using Domain.Abstracts;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Measure : BaseEntity
    {
        public Measure(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(20)] public string Unit { get; set; } = nameof(MeasurementUnitConstants.Weight_Pounds);
        [Required][MaxLength(20)] public string MeasurementType { get; set; } = nameof(MeasurementTypeConstants.Weight);
        [Required][MaxLength(20)] public string Method { get; set; } = nameof(MeasurementMethodConstants.Direct);
        [Required][MaxLength(40)] public string Name { get; set; }
    }
}
