using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class LivestockStatus : BaseEntity
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
    }
}
