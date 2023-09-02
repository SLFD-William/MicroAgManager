using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class DutyModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required] public int DaysDue { get; set; }
        [Required][MaxLength(20)] public string DutyType { get; set; }
        [Required][Range(1,long.MaxValue)] public long DutyTypeId { get; set; }
        [Required][MaxLength(20)] public string Relationship { get; set; }
        [MaxLength(1)] public string? Gender { get; set; }
        [Required] public bool SystemRequired { get; set; }

        [ForeignKey(nameof(LivestockAnimalModel))] public long? LivestockAnimalId { get; set; }

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
                DutyType = duty.DutyType,
                DutyTypeId = duty.DutyTypeId,
                Relationship = duty.Relationship,
                Gender = duty.Gender,
                SystemRequired = duty.SystemRequired,
                LivestockAnimalId = duty.LivestockAnimal?.Id,
                Milestones=duty.Milestones.Select(MilestoneModel.Create).ToList() ?? new List<MilestoneModel?>(),
                ScheduledDuties=duty.ScheduledDuties.Select(ScheduledDutyModel.Create).ToList() ?? new List<ScheduledDutyModel?>(),
                Events =duty.Events.Select(EventModel.Create).ToList() ?? new List<EventModel?>()
            }) as DutyModel;
            return model;
        }
        public Duty MapToEntity(Duty duty)
        { 
            duty.DutyType= DutyType;
            duty.DutyTypeId= DutyTypeId;
            duty.Name= Name;
            duty.DaysDue= DaysDue;
            duty.Relationship= Relationship;
            duty.Gender= Gender;
            duty.SystemRequired= SystemRequired;
            if(duty.LivestockAnimal is not null && LivestockAnimalId.HasValue)
                duty.LivestockAnimal.Id= LivestockAnimalId.Value;
            if (duty.Events?.Any() ?? false)
                foreach (var plot in duty.Events)
                    Events.FirstOrDefault(p => p?.Id == plot.Id)?.MapToEntity(plot);
            if (duty.Milestones?.Any() ?? false)
                foreach (var plot in duty.Milestones)
                    Milestones.FirstOrDefault(p => p?.Id == plot.Id)?.MapToEntity(plot);
            if (duty.ScheduledDuties?.Any() ?? false)
                foreach (var plot in duty.ScheduledDuties)
                    ScheduledDuties.FirstOrDefault(p => p?.Id == plot.Id)?.MapToEntity(plot);

            return duty;
        }
    }
}
