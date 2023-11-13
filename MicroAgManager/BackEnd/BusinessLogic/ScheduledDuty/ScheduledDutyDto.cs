using Domain.Models;

namespace BackEnd.BusinessLogic.ScheduledDuty
{
    public class ScheduledDutyDto
    {
        public ScheduledDutyDto(long count, ICollection<ScheduledDutyModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<ScheduledDutyModel> Models { get; set; }
    }
}
