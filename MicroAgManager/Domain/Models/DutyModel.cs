using Domain.Abstracts;
using Domain.Entity;
using Domain.Interfaces;
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
        [MaxLength(255)] public string ProcedureLink { get; set; }


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
                ProcedureLink = duty.ProcedureLink,
                Milestones=duty.Milestones.Select(m=>MilestoneModel.Create(m)).ToList() ?? new List<MilestoneModel?>(),
                ScheduledDuties=duty.ScheduledDuties.Select(d=> ScheduledDutyModel.Create(d)).ToList() ?? new List<ScheduledDutyModel?>(),
                Events =duty.Events.Select(d => EventModel.Create(d)).ToList() ?? new List<EventModel?>()
            }) as DutyModel;
            return model;
        }
        public override BaseModel Map(BaseModel duty)
        {
            if (duty == null || duty is not DutyModel) return null;
            ((DutyModel)duty).Command = Command;
            ((DutyModel)duty).CommandId = CommandId;
            ((DutyModel)duty).RecipientType = RecipientType;
            ((DutyModel)duty).RecipientTypeId = RecipientTypeId;
            ((DutyModel)duty).Name = Name;
            ((DutyModel)duty).DaysDue = DaysDue;
            ((DutyModel)duty).Relationship = Relationship;
            ((DutyModel)duty).Gender = Gender;
            ((DutyModel)duty).ProcedureLink =ProcedureLink;
            ((DutyModel)duty).SystemRequired = SystemRequired;
            return duty;
        }
        public override BaseEntity Map(BaseEntity duty)
        {
            if(duty == null || duty is not Duty) return null;

            ((Duty)duty).Command = Command;
            ((Duty)duty).CommandId = CommandId;
            ((Duty)duty).RecipientType = RecipientType;
            ((Duty)duty).RecipientTypeId = RecipientTypeId;
            ((Duty)duty).Name = Name;
            ((Duty)duty).DaysDue = DaysDue;
            ((Duty)duty).Relationship = Relationship;
            ((Duty)duty).Gender = Gender;
            ((Duty)duty).SystemRequired = SystemRequired;
            ((Duty)duty).ProcedureLink = ProcedureLink;
            duty.ModifiedOn = DateTime.UtcNow;
            return duty;
        }
    }
}
