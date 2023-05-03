using Domain.Entity;
using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class FarmLocationModel:BaseModel
    {
        [ForeignKey(nameof(TenantModel))]
        [Required] public Guid TenantId { get; set; }
        [Required] public string? Name { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        [Required] public string? StreetAddress { get; set; }
        [Required] public string? City { get; set; }
        [Required] public string? State { get; set; }
        [Required] public string? Zip { get; set; }
        public string? Country { get; set; }
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
                TenantId=farm.TenantId
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

            return farm;
        }
    }
}
