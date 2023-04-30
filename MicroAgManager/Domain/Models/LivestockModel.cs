using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockModel : BaseModel
    {
        [ForeignKey(nameof(LivestockModel))] public long? MotherId { get; set; }
        [ForeignKey(nameof(LivestockModel))] public long? FatherId { get; set; }

        [Required][ForeignKey(nameof(LivestockBreedModel))]  public long LivestockBreedId { get; set; }
        [Required] [MaxLength(40)]public string Name { get; set; }
        [Required] public DateTime Birthdate { get; set; }
        [Required][MaxLength(40)] public string Gender { get; set; }
        [MaxLength(40)] public string Variety { get; set; }
        [MaxLength(255)] public string Description { get; set; }
        [Required] public bool BeingManaged { get; set; }
        [Required] public bool BornDefective { get; set; }
        //[RequiredIf("BornDefective", "True")]
        [MaxLength(255)] public string BirthDefect { get; set; }
        [Required] public bool Sterile { get; set; }
        [Required] public bool InMilk { get; set; }
        [Required] public bool BottleFed { get; set; }
        [Required] public bool ForSale { get; set; }
        public virtual ICollection<LivestockStatusModel?> Statuses { get; set; } = new List<LivestockStatusModel?>();
        public virtual ICollection<LandPlotModel?> Locations { get; set; } = new List<LandPlotModel?>();
        public static LivestockModel? Create(Livestock livestock)
        {
            var model = PopulateBaseModel(livestock, new LivestockModel
            {
                MotherId = livestock.MotherId,
                FatherId = livestock.FatherId,
                LivestockBreedId = livestock.Breed.Id,
                BeingManaged = livestock.BeingManaged,
                Birthdate = livestock.Birthdate,
                BirthDefect = livestock.BirthDefect,
                BornDefective = livestock.BornDefective,
                BottleFed = livestock.BottleFed,
                ForSale = livestock.ForSale,
                Description = livestock.Description,
                Gender = livestock.Gender,
                InMilk = livestock.InMilk,
                Variety = livestock.Variety,
                Sterile = livestock.Sterile,
                Name = livestock.Name,
                Statuses = livestock.Statuses.Select(LivestockStatusModel.Create).ToList() ?? new List<LivestockStatusModel?>(),
                Locations = livestock.Locations.Select(LandPlotModel.Create).ToList() ?? new List<LandPlotModel?>()
            }) as LivestockModel;
            return model;
        }
        public Livestock MapToEntity(Livestock entity)
        {
            entity.MotherId = MotherId;
            entity.FatherId = FatherId;
            entity.Breed.Id = LivestockBreedId;
            entity.BeingManaged = BeingManaged;
            entity.Birthdate = Birthdate;
            entity.BirthDefect = BirthDefect;
            entity.BornDefective = BornDefective;
            entity.BottleFed = BottleFed;
            entity.Description = Description;
            entity.ForSale = ForSale;
            entity.Gender = Gender;
            entity.InMilk = InMilk;
            entity.Variety = Variety;   
            entity.Sterile = Sterile;
            entity.Name = Name;
            if (entity.Statuses?.Any() ?? false)
                foreach (var breed in entity.Statuses)
                    Statuses?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            if(entity.Locations?.Any() ?? false)
                foreach (var breed in entity.Locations)
                    Locations?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);


            return entity;
        }
    }
}
