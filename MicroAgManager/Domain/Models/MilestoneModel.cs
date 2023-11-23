using Domain.Abstracts;
using Domain.Entity;
using Domain.Interfaces;
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
        public static MilestoneModel? Create(Milestone? milestone,IMicroAgManagementDbContext db)
        {
            if (milestone == null) return null;
            var model = PopulateBaseModel(milestone, new MilestoneModel
            {
                RecipientTypeId = milestone.RecipientTypeId,
                RecipientType = milestone.RecipientType,
                Name = milestone.Name,
                Description=milestone.Description,
                SystemRequired = milestone.SystemRequired,
                Events= milestone.Events?.Select(d => EventModel.Create(d, db)).ToList() ?? new List<EventModel?>(),
                Duties= milestone.Duties?.Select(d => DutyModel.Create(d, db)).ToList() ?? new List<DutyModel?>()
            }) as MilestoneModel;
            return model;
        }

        public override BaseModel Map(BaseModel milestone)
        {
            if (milestone == null || milestone is not MilestoneModel) return null;
            ((MilestoneModel)milestone).Name = Name;
            ((MilestoneModel)milestone).Description = Description;
            ((MilestoneModel)milestone).SystemRequired = SystemRequired;
            ((MilestoneModel)milestone).RecipientTypeId = RecipientTypeId;
            ((MilestoneModel)milestone).RecipientType = RecipientType;
            return milestone;
        }

        public override BaseEntity Map(BaseEntity milestone)
        {
            if (milestone == null || milestone is not Milestone) return null;
            ((Milestone)milestone).Name = Name;
            ((Milestone)milestone).Description = Description;
            ((Milestone)milestone).SystemRequired = SystemRequired;
            ((Milestone)milestone).RecipientTypeId = RecipientTypeId;
            ((Milestone)milestone).RecipientType = RecipientType;
            milestone.ModifiedOn = DateTime.UtcNow;
            return milestone;
        }
    }
}
