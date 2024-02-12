using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class DutyModel : BaseModel,IDuty
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
        [MaxLength(255)] public string? ProcedureLink { get; set; }

        [NotMapped] public string RecipientTypeItem { get; private set; } = string.Empty;
        [NotMapped] public string CommandItem { get; private set; } = string.Empty;
        public virtual ICollection<EventModel?> Events { get; set; } = new List<EventModel?>();
        public virtual ICollection<MilestoneModel?> Milestones { get; set; } = new List<MilestoneModel?>();
        public virtual ICollection<ChoreModel?> Chores { get; set; } = new List<ChoreModel?>();
        public virtual ICollection<ScheduledDutyModel?> ScheduledDuties { get; set; } = new List<ScheduledDutyModel?>();
        [NotMapped] DateTime IDuty.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
       [NotMapped] ICollection<IChore>? IDuty.Chores { get => Chores as ICollection<IChore>; set => Chores=value as ICollection<ChoreModel?> ?? new List<ChoreModel?>(); }
       [NotMapped] ICollection<IEvent>? IDuty.Events { get => Events as ICollection<IEvent>; set => Events = value as ICollection<EventModel?> ?? new List<EventModel?>(); }
       [NotMapped] ICollection<IMilestone>? IDuty.Milestones { get => Milestones as ICollection<IMilestone>; set => Milestones = value as ICollection<MilestoneModel?> ?? new List<MilestoneModel?>(); }
       [NotMapped] ICollection<IScheduledDuty>? IDuty.ScheduledDuties { get => ScheduledDuties as ICollection<IScheduledDuty>; set => ScheduledDuties = value as ICollection<ScheduledDutyModel?> ?? new List<ScheduledDutyModel?>(); }

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
                Milestones=duty.Milestones.Select(MilestoneModel.Create).ToList() ?? new List<MilestoneModel?>(),
                ScheduledDuties=duty.ScheduledDuties.Select(ScheduledDutyModel.Create).ToList() ?? new List<ScheduledDutyModel?>(),
                Events =duty.Events.Select(EventModel.Create).ToList() ?? new List<EventModel?>(),
                Chores = duty.Chores.Select(ChoreModel.Create).ToList() ?? new List<ChoreModel?>()
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
            ((DutyModel)duty).EntityModifiedOn = EntityModifiedOn;
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
        public void PopulateDynamicRelations(IFrontEndDbContext db)
        {
            if (RecipientType == nameof(LivestockAnimal))
                RecipientTypeItem = db.LivestockAnimals.Find(RecipientTypeId)?.Name;
            if (Command == DutyCommandConstants.Measurement)
                CommandItem = db.Measures.Find(CommandId)?.Name;
            if (Command == DutyCommandConstants.Treatment)
                CommandItem = db.Treatments.Find(CommandId)?.Name;
            if (Command == DutyCommandConstants.Registration)
                CommandItem = db.Registrars.Find(CommandId)?.Name;
        }
    }
}
