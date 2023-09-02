using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockStatusModel : BaseModel
    {
        [Required]
        [ForeignKey(nameof(LivestockTypeModel))]
        public long LivestockTypeId { get; set; }
        [Required] [MaxLength(40)]public string Status { get; set; }
        [Required] public bool DefaultStatus { get; set; }
        [Required][MaxLength(10)] public string BeingManaged { get; set; }
        [Required][MaxLength(10)] public string Sterile { get; set; }
        [Required][MaxLength(10)] public string InMilk { get; set; }
        [Required][MaxLength(10)] public string BottleFed { get; set; }
        [Required][MaxLength(10)] public string ForSale { get; set; }
        public virtual ICollection<LivestockModel> Livestocks { get; set; } = new List<LivestockModel>();
        public static LivestockStatusModel? Create(LivestockStatus livestockType)
        {
            var model = PopulateBaseModel(livestockType, new LivestockStatusModel
            {
                Status = livestockType.Status,
                BeingManaged = livestockType.BeingManaged,
                Sterile = livestockType.Sterile, 
                InMilk = livestockType.InMilk, 
                BottleFed = livestockType.BottleFed,
                ForSale = livestockType.ForSale,
                DefaultStatus = livestockType.DefaultStatus,
                LivestockTypeId=livestockType.LivestockType.Id,
                Livestocks = livestockType.Livestocks.Select(LivestockModel.Create).ToList() ?? new List<LivestockModel>()
            }) as LivestockStatusModel;
            return model;
        }
        public LivestockStatus MapToEntity(LivestockStatus entity)
        {
            entity.Status = Status;
            entity.BeingManaged = BeingManaged;
            entity.InMilk = InMilk;
            entity.BottleFed = BottleFed;
            entity.ForSale = ForSale;
            entity.Sterile = Sterile;
            entity.DefaultStatus = DefaultStatus;
            entity.LivestockType.Id=LivestockTypeId;
            if (entity.Livestocks.Any())
                foreach (var subplot in entity.Livestocks)
                    Livestocks.FirstOrDefault(p => p?.Id == subplot.Id)?.MapToEntity(subplot);
            return entity;
        }
    }
}
