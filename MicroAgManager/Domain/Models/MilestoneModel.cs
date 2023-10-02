using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class MilestoneModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        [Required] public bool SystemRequired { get; set; }
        [MaxLength(40)] public string RecipientType { get; set; }
        [Required]
        [Range(1, long.MaxValue)] public long RecipientTypeId { get; set; }
        public virtual ICollection<EventModel?> Events { get; set; } = new List<EventModel?>();
        public virtual ICollection<DutyModel?> Duties { get; set; } = new List<DutyModel?>();
        public static MilestoneModel? Create(Milestone? milestone)
        {
            if (milestone == null) return null;
            var model = PopulateBaseModel(milestone, new MilestoneModel
            {
                RecipientTypeId = milestone.RecipientTypeId,
                RecipientType = milestone.RecipientType,
                Name = milestone.Name,
                Description=milestone.Description,
                SystemRequired = milestone.SystemRequired,
                Events= milestone.Events?.Select(EventModel.Create).ToList() ?? new List<EventModel?>(),
                Duties= milestone.Duties?.Select(DutyModel.Create).ToList() ?? new List<DutyModel?>()
            }) as MilestoneModel;
            return model;
        }
        public Milestone MapToEntity(Milestone milestone)
        {
            milestone.Name= Name;
            milestone.Description = Description;
            milestone.SystemRequired= SystemRequired;
            milestone.RecipientTypeId = RecipientTypeId;
            milestone.RecipientType = RecipientType;
            milestone.ModifiedOn = DateTime.UtcNow;
            return milestone;
        }
    }
}
