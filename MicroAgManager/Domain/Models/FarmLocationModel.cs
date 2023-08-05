using Domain.Entity;
using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FarmLocationModel:BaseModel
    {
        [Required] public Guid TenantId { get; set; }
        [Required] public string? Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        [Required] public string? StreetAddress { get; set; }
        [Required] public string? City { get; set; }
        [Required] public string? State { get; set; }
        [Required] public string? Zip { get; set; }
        public string? Country { get; set; }
        [MaxLength(2)]
        public string? CountryCode { get; set; }
        public static FarmLocationModel? Create(FarmLocation? farm)
        {
            if (farm == null) return null;
            var model = PopulateBaseModel(farm, new FarmLocationModel
            {
                Name = farm.Name,
                Longitude = farm.Longitude,
                Latitude = farm.Latitude,
                StreetAddress = farm.StreetAddress,
                City = farm.City,
                State = farm.State,
                Zip = farm.Zip,
                Country = farm.Country,
                TenantId=farm.TenantId,
                CountryCode=farm.CountryCode
            }) as FarmLocationModel;
            return model;
        }
        public FarmLocation MapToEntity(FarmLocation farm)
        {
            farm.Longitude =Longitude;
            farm.Latitude =Latitude;
            farm.StreetAddress =StreetAddress;
            farm.City =City;
            farm.State = State;
            farm.Zip = Zip;
            farm.Country = Country;
            farm.Name = Name;
            farm.CountryCode = CountryCode;
            return farm;
        }
    }
}
