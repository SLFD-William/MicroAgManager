using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockStatusModel : BaseModel
    {
        [Required]
        [ForeignKey(nameof(Animal))]
        public long LivestockAnimalId { get; set; }
        public virtual LivestockAnimalModel Animal { get; set; }
        [Required] [MaxLength(40)]public string Status { get; set; }
        [Required] public bool DefaultStatus { get; set; }
        [Required][MaxLength(10)] public string BeingManaged { get; set; }
        [Required][MaxLength(10)] public string Sterile { get; set; }
        [Required][MaxLength(10)] public string InMilk { get; set; }
        [Required][MaxLength(10)] public string BottleFed { get; set; }
        [Required][MaxLength(10)] public string ForSale { get; set; }
        public virtual ICollection<LivestockModel> Livestocks { get; set; } = new List<LivestockModel>();
        public static LivestockStatusModel? Create(LivestockStatus LivestockStatus)
        {
            var model = PopulateBaseModel(LivestockStatus, new LivestockStatusModel
            {
                Status = LivestockStatus.Status,
                BeingManaged = LivestockStatus.BeingManaged,
                Sterile = LivestockStatus.Sterile, 
                InMilk = LivestockStatus.InMilk, 
                BottleFed = LivestockStatus.BottleFed,
                ForSale = LivestockStatus.ForSale,
                DefaultStatus = LivestockStatus.DefaultStatus,
                LivestockAnimalId=LivestockStatus.LivestockAnimalId,
                Livestocks = LivestockStatus.Livestocks.Select(LivestockModel.Create).ToList() ?? new List<LivestockModel>()
            }) as LivestockStatusModel;
            return model;
        }
        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockStatusModel) return null;
            ((LivestockStatusModel) entity).Status = Status;
            ((LivestockStatusModel)entity).BeingManaged = BeingManaged;
            ((LivestockStatusModel)entity).InMilk = InMilk;
            ((LivestockStatusModel)entity).BottleFed = BottleFed;
            ((LivestockStatusModel)entity).ForSale = ForSale;
            ((LivestockStatusModel)entity).Sterile = Sterile;
            ((LivestockStatusModel)entity).DefaultStatus = DefaultStatus;
            ((LivestockStatusModel)entity).LivestockAnimalId = LivestockAnimalId;
            ((LivestockStatusModel)entity).EntityModifiedOn = EntityModifiedOn;
            return entity;
        }
        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockStatus) return null;
            ((LivestockStatus)entity).Status = Status;
            ((LivestockStatus)entity).BeingManaged = BeingManaged;
            ((LivestockStatus)entity).InMilk = InMilk;
            ((LivestockStatus)entity).BottleFed = BottleFed;
            ((LivestockStatus)entity).ForSale = ForSale;
            ((LivestockStatus)entity).Sterile = Sterile;
            ((LivestockStatus)entity).DefaultStatus = DefaultStatus;
            ((LivestockStatus)entity).LivestockAnimalId = LivestockAnimalId;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
