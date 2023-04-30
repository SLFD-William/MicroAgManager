using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class EventModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string Color { get; set; }
        [Required] public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<DutyModel?> Duties { get; set; } = new List<DutyModel?>();
        public virtual ICollection<MilestoneModel?> Milestones { get; set; } = new List<MilestoneModel?>();
        public virtual ICollection<ScheduledDutyModel?> ScheduledDuties { get; set; } = new List<ScheduledDutyModel?>();

        public static EventModel? Create(Event? duty)
        {
            if (duty == null) return null;
            var model = PopulateBaseModel(duty, new EventModel
            {
                Name = duty.Name,
                Color = duty.Color,
                StartDate = duty.StartDate,
                EndDate = duty.EndDate,
                Duties = duty.Duties?.Select(DutyModel.Create).ToList() ?? new List<DutyModel?>(),
                Milestones = duty.Milestones?.Select(MilestoneModel.Create).ToList() ?? new List<MilestoneModel?>(),
                ScheduledDuties = duty.ScheduledDuties?.Select(ScheduledDutyModel.Create).ToList() ?? new List<ScheduledDutyModel?>()
            }) as EventModel;
            return model;
        }
        public Event MapToEntity(Event duty)
        {
            duty.Name= Name;
            duty.Color= Color;
            duty.StartDate= StartDate;
            duty.EndDate= EndDate;
            if (duty.Duties?.Any() ?? false)
                foreach (var plot in duty.Duties)
                    Duties.FirstOrDefault(p => p?.Id == plot.Id)?.MapToEntity(plot);
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
