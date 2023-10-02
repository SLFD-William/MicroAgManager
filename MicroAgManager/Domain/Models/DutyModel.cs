using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class DutyModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required] public int DaysDue { get; set; }
        [Required][MaxLength(20)] public string Command { get; set; }
        [Required][Range(0,long.MaxValue)] public long CommandId { get; set; }
        [Required]
        [MaxLength(40)] public string RecipientType { get; set; }
        [Required]
        [Range(0,long.MaxValue)] public long RecipientTypeId { get; set; }

        [Required][MaxLength(20)] public string Relationship { get; set; }
        [MaxLength(1)] public string? Gender { get; set; }
        [Required] public bool SystemRequired { get; set; }


        public virtual ICollection<EventModel?> Events { get; set; } = new List<EventModel?>();
        public virtual ICollection<MilestoneModel?> Milestones { get; set; } = new List<MilestoneModel?>();
        public virtual ICollection<ScheduledDutyModel?> ScheduledDuties { get; set; } = new List<ScheduledDutyModel?>();

        public static DutyModel? Create(Duty? duty)
        {
            if (duty == null) return null;
            var model = PopulateBaseModel(duty, new DutyModel
            {
                Name = duty.Name,
                DaysDue = duty.DaysDue,
                Command = duty.Command,
                CommandId = duty.CommandId,
                RecipientType = duty.RecipientType,
                RecipientTypeId = duty.RecipientTypeId,
                Relationship = duty.Relationship,
                Gender = duty.Gender,
                SystemRequired = duty.SystemRequired,
                Milestones=duty.Milestones.Select(MilestoneModel.Create).ToList() ?? new List<MilestoneModel?>(),
                ScheduledDuties=duty.ScheduledDuties.Select(ScheduledDutyModel.Create).ToList() ?? new List<ScheduledDutyModel?>(),
                Events =duty.Events.Select(EventModel.Create).ToList() ?? new List<EventModel?>()
            }) as DutyModel;
            return model;
        }
        public Duty MapToEntity(Duty duty)
        { 
            duty.Command= Command;
            duty.CommandId= CommandId;
            duty.RecipientType = RecipientType;
            duty.RecipientTypeId = RecipientTypeId;
            duty.Name= Name;
            duty.DaysDue= DaysDue;
            duty.Relationship= Relationship;
            duty.Gender= Gender;
            duty.SystemRequired= SystemRequired;

            duty.ModifiedOn = DateTime.UtcNow;
            return duty;
        }
    }
}
