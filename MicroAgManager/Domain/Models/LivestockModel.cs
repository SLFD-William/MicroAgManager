using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockModel : BaseModel
    {
        [ForeignKey(nameof(Mother))] public long? MotherId { get; set; }
        [ForeignKey(nameof(Father))] public long? FatherId { get; set; }
        [Required][ForeignKey(nameof(Status))] public long? StatusId { get; set; }
        [ForeignKey(nameof(LandPlotModel))] public long? LocationId { get; set; }
        [Required][ForeignKey(nameof(Breed))]  public long LivestockBreedId { get; set; }
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
        public virtual LivestockModel? Mother { get; set; }
        public virtual LivestockModel? Father { get; set; }
        public virtual LivestockStatusModel? Status { get; set; }
        public virtual LivestockBreedModel Breed { get; set; }

        [NotMapped]public string CurrentStatus => (StatusId.HasValue &&  Status!=null) ? Status.Status:string.Empty;
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
                BatchNumber = livestock.BatchNumber,
                StatusId = livestock.StatusId,
                LocationId = livestock.LocationId
            }) as LivestockModel;
            return model;
        }
        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockModel) return null;
            ((LivestockModel)entity).MotherId = MotherId;
            ((LivestockModel)entity).FatherId = FatherId;
            ((LivestockModel)entity).LivestockBreedId = LivestockBreedId;
            ((LivestockModel)entity).BeingManaged = BeingManaged;
            ((LivestockModel)entity).Birthdate = Birthdate;
            ((LivestockModel)entity).BirthDefect = BirthDefect;
            ((LivestockModel)entity).BornDefective = BornDefective;
            ((LivestockModel)entity).BottleFed = BottleFed;
            ((LivestockModel)entity).Description = Description;
            ((LivestockModel)entity).ForSale = ForSale;
            ((LivestockModel)entity).Gender = Gender;
            ((LivestockModel)entity).InMilk = InMilk;
            ((LivestockModel)entity).Variety = Variety;
            ((LivestockModel)entity).Sterile = Sterile;
            ((LivestockModel)entity).Name = Name;
            ((LivestockModel)entity).BatchNumber = BatchNumber;
            ((LivestockModel)entity).StatusId = StatusId;
            ((LivestockModel)entity).LocationId = LocationId;
            return entity;
        }
        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not Livestock) return null;
            ((Livestock)entity).MotherId = MotherId;
            ((Livestock)entity).FatherId = FatherId;
            ((Livestock)entity).LivestockBreedId = LivestockBreedId;
            ((Livestock)entity).BeingManaged = BeingManaged;
            ((Livestock)entity).Birthdate = Birthdate;
            ((Livestock)entity).BirthDefect = BirthDefect;
            ((Livestock)entity).BornDefective = BornDefective;
            ((Livestock)entity).BottleFed = BottleFed;
            ((Livestock)entity).Description = Description;
            ((Livestock)entity).ForSale = ForSale;
            ((Livestock)entity).Gender = Gender;
            ((Livestock)entity).InMilk = InMilk;
            ((Livestock)entity).Variety = Variety;
            ((Livestock)entity).Sterile = Sterile;
            ((Livestock)entity).Name = Name;
            ((Livestock)entity).BatchNumber = BatchNumber;
            ((Livestock)entity).StatusId = StatusId;
            ((Livestock)entity).LocationId = LocationId;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
