using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class FarmLocation : BaseEntity
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
        public virtual ICollection<LandPlot> Plots { get; set; }
    }
}
