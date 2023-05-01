using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class MilestoneModel : BaseModel
    {
        [Required][MaxLength(40)] public string Subcategory { get; set; }
        [Required] public bool SystemRequired { get; set; }
        [ForeignKey(nameof(LivestockTypeModel))] public long? LivestockTypeId { get; set; }
        public virtual ICollection<EventModel?> Events { get; set; } = new List<EventModel?>();
        public virtual ICollection<DutyModel?> Duties { get; set; } = new List<DutyModel?>();
        public static MilestoneModel? Create(Milestone? milestone)
        {
            if (milestone == null) return null;
            var model = PopulateBaseModel(milestone, new MilestoneModel
            {
                LivestockTypeId= milestone.LivestockType?.Id,
                Subcategory = milestone.Subcategory,
                SystemRequired = milestone.SystemRequired,
                Events= milestone.Events?.Select(EventModel.Create).ToList() ?? new List<EventModel?>(),
                Duties= milestone.Duties?.Select(DutyModel.Create).ToList() ?? new List<DutyModel?>()
            }) as MilestoneModel;
            return model;
        }
        public Milestone MapToEntity(Milestone milestone)
        {
            milestone.Subcategory= Subcategory;
            milestone.SystemRequired= SystemRequired;
            if(LivestockTypeId.HasValue && milestone.LivestockType is not null) milestone.LivestockType.Id= LivestockTypeId.Value;
            if (milestone.Events?.Any() ?? false)
                foreach (var plot in milestone.Events)
                    Events.FirstOrDefault(p => p?.Id == plot.Id)?.MapToEntity(plot);
            if (milestone.Duties?.Any() ?? false)
                foreach (var plot in milestone.Duties)
                    Duties.FirstOrDefault(p => p?.Id == plot.Id)?.MapToEntity(plot);
            return milestone;
        }
    }
}
