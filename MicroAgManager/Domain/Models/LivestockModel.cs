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
        [Required][ForeignKey(nameof(LivestockStatusModel))] public long? StatusId { get; set; }
        [ForeignKey(nameof(LandPlotModel))] public long? LocationId { get; set; }
        [Required][ForeignKey(nameof(LivestockBreedModel))]  public long LivestockBreedId { get; set; }
        [Required] [MaxLength(40)]public string Name { get; set; }
        [Required][MaxLength(40)] public string BatchNumber { get; set; }
        [Required] public DateTime Birthdate { get; set; }
        [Required][MaxLength(40)] public string Gender { get; set; }
        [MaxLength(40)] public string Variety { get; set; }
        [MaxLength(255)] public string Description { get; set; }
        public bool BeingManaged { get; set; } = true;
        public bool BornDefective { get; set; } = false;
        //[RequiredIf("BornDefective", "True")]
        [MaxLength(255)] public string BirthDefect { get; set; }=string.Empty;
        public bool Sterile { get; set; } = false;
        public bool InMilk { get; set; } = false;
        public bool BottleFed { get; set; } = false;
        public bool ForSale { get; set; } = true;
        public static LivestockModel Create(Livestock livestock)
        {
            var model = PopulateBaseModel(livestock, new LivestockModel
            {
                MotherId = livestock.MotherId,
                FatherId = livestock.FatherId,
                LivestockBreedId = livestock.LivestockBreedId,
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
                BatchNumber=livestock.BatchNumber,
                StatusId = livestock.StatusId,
                LocationId = livestock.LocationId
            }) as LivestockModel;
            return model;
        }
        public Livestock MapToEntity(Livestock entity)
        {
            entity.MotherId = MotherId;
            entity.FatherId = FatherId;
            entity.LivestockBreedId = LivestockBreedId;
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
            entity.BatchNumber = BatchNumber;
            entity.StatusId = StatusId;
            entity.LocationId = LocationId;
            return entity;
        }
    }
}
