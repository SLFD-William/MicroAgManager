using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class MilestoneModel : BaseModel,IMilestone
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        [Required] public bool SystemRequired { get; set; }
        [MaxLength(40)] public string RecipientType { get; set; }
        [Required]
        [Range(1, long.MaxValue)] public long RecipientTypeId { get; set; }
        public virtual ICollection<EventModel?> Events { get; set; } = new List<EventModel?>();
        public virtual ICollection<DutyModel?> Duties { get; set; } = new List<DutyModel?>();
        [NotMapped] DateTime IMilestone.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value == EntityModifiedOn ? EntityModifiedOn : EntityModifiedOn; }
        [NotMapped] ICollection<IDuty>? IMilestone.Duties { get => Duties as ICollection<IDuty>; set => Duties = value as ICollection<DutyModel?> ?? new List<DutyModel?>(); }
       [NotMapped] ICollection<IEvent>? IMilestone.Events { get => Events as ICollection<IEvent>; set => Events = value as ICollection<EventModel?> ?? new List<EventModel?>(); }

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

        public override BaseModel Map(BaseModel milestone)
        {
            if (milestone == null || milestone is not MilestoneModel) return null;
            ((MilestoneModel)milestone).Name = Name;
            ((MilestoneModel)milestone).Description = Description;
            ((MilestoneModel)milestone).SystemRequired = SystemRequired;
            ((MilestoneModel)milestone).RecipientTypeId = RecipientTypeId;
            ((MilestoneModel)milestone).RecipientType = RecipientType;
            ((MilestoneModel)milestone).EntityModifiedOn = EntityModifiedOn;
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
