using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Domain.Entity
{
    public interface IMeasurement
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        DateTime DatePerformed { get; set; }
        long MeasureId { get; set; }
       IUnit MeasurementUnit { get; set; }
        long MeasurementUnitId { get; set; }
        string Notes { get; set; }
        long RecipientId { get; set; }
        string RecipientType { get; set; }
        long RecipientTypeId { get; set; }
        decimal Value { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    [Index(nameof(RecipientType), nameof(RecipientTypeId))]
    public class Measurement : BaseEntity, IHasRecipient, IMeasurement
    {
        public Measurement(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Measure")] public long MeasureId { get; set; }
        public virtual Measure Measure { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
        [Precision(18, 3)][Required] public decimal Value { get; set; }
        [Required][ForeignKey("MeasurementUnit")] public long MeasurementUnitId { get; set; }
        public virtual Unit MeasurementUnit { get; set; }
        public string Notes { get; set; }
        [Required] public DateTime DatePerformed { get; set; }
      [NotMapped]  IUnit IMeasurement.MeasurementUnit { get => MeasurementUnit; set => MeasurementUnit=value as Unit ?? MeasurementUnit; }
    }
}
