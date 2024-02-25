using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestockStatus
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string BeingManaged { get; set; }
        string BottleFed { get; set; }
        bool DefaultStatus { get; set; }
       ICollection<ILivestockFeedServing>? FeedServings { get; set; }
        string ForSale { get; set; }
        string InMilk { get; set; }
        long LivestockAnimalId { get; set; }
       ICollection<ILivestock>? Livestocks { get; set; }
        string Status { get; set; }
        string Sterile { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockStatus : BaseEntity, ILivestockStatus
    {
        public LivestockStatus(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("LivestockAnimal")] public long LivestockAnimalId { get; set; }
        [Required][MaxLength(40)] public string Status { get; set; }
        [Required] public bool DefaultStatus { get; set; }
        [Required][MaxLength(10)] public string InMilk { get; set; }
        [Required][MaxLength(10)] public string BeingManaged { get; set; }
        [Required][MaxLength(10)] public string Sterile { get; set; }
        [Required][MaxLength(10)] public string BottleFed { get; set; }
        [Required][MaxLength(10)] public string ForSale { get; set; }
        public virtual LivestockAnimal LivestockAnimal { get; set; }
        public virtual ICollection<LivestockFeedServing> FeedServings { get; set; } = new List<LivestockFeedServing>();
        public virtual ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();
         ICollection<ILivestockFeedServing>? ILivestockStatus.FeedServings { get => FeedServings as ICollection<ILivestockFeedServing>; set =>FeedServings=value as ICollection<LivestockFeedServing>?? new List<LivestockFeedServing>(); }
         ICollection<ILivestock>? ILivestockStatus.Livestocks { get => Livestocks as ICollection<ILivestock>; set => Livestocks = value as ICollection<Livestock> ?? new List<Livestock>(); }
        }
}
