using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IFarmLocation
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string? City { get; set; }
        string? Country { get; set; }
        string? CountryCode { get; set; }
        double? Latitude { get; set; }
        double? Longitude { get; set; }
        string Name { get; set; }
       ICollection<ILandPlot>? Plots { get; set; }
        string? State { get; set; }
        string? StreetAddress { get; set; }
        string? Zip { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class FarmLocation : BaseEntity, IFarmLocation
    {
        public FarmLocation(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(50)] public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        [MaxLength(2)]
        public string? CountryCode { get; set; }
        public virtual ICollection<LandPlot> Plots { get; set; } = new List<LandPlot>();
       [NotMapped] ICollection<ILandPlot>? IFarmLocation.Plots { get => Plots as ICollection<ILandPlot>; set => Plots=value as ICollection<LandPlot> ?? new List<LandPlot>(); }
    }
}
