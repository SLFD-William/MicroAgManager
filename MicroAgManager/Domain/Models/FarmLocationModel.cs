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
        public virtual ICollection<LandPlotModel> Plots { get; set; } = new List<LandPlotModel>();
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


        public override BaseModel Map(BaseModel farm)
        {
            if (farm is null | farm is not FarmLocationModel) return null;
            ((FarmLocationModel)farm).Longitude = Longitude;
            ((FarmLocationModel)farm).Latitude = Latitude;
            ((FarmLocationModel)farm).StreetAddress = StreetAddress;
            ((FarmLocationModel)farm).City = City;
            ((FarmLocationModel)farm).State = State;
            ((FarmLocationModel)farm).Zip = Zip;
            ((FarmLocationModel)farm).Country = Country;
            ((FarmLocationModel)farm).Name = Name;
            ((FarmLocationModel)farm).CountryCode = CountryCode;
            return farm;
        }

        public override BaseEntity Map(BaseEntity farm)
        {
            if (farm is null | farm is not FarmLocation) return null;
            ((FarmLocation)farm).Longitude = Longitude;
            ((FarmLocation)farm).Latitude = Latitude;
            ((FarmLocation)farm).StreetAddress = StreetAddress;
            ((FarmLocation)farm).City = City;
            ((FarmLocation)farm).State = State;
            ((FarmLocation)farm).Zip = Zip;
            ((FarmLocation)farm).Country = Country;
            ((FarmLocation)farm).Name = Name;
            ((FarmLocation)farm).CountryCode = CountryCode;
            farm.ModifiedOn = DateTime.UtcNow;
            return farm;
        }
    }
}
