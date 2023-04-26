using Domain.Entity;
using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FarmLocationModel:BaseModel
    {
        [Required] public string? Name { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        [Required] public string? StreetAddress { get; set; }
        [Required] public string? City { get; set; }
        [Required] public string? State { get; set; }
        [Required] public string? Zip { get; set; }
        public string? Country { get; set; }
        public virtual ICollection<LandPlotModel?>? Plots { get; set; }
        public static FarmLocationModel? Create(FarmLocation? farm)
        {
            if (farm == null)
                return null;
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
                Plots=farm.Plots?.Select(LandPlotModel.Create).ToList() ?? new List<LandPlotModel?>()
            }) as FarmLocationModel;
            return model;
        }
    }
}
