using Domain.ValueObjects;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class DutyEventDto
    {
        public DutyEventDto(long count, ICollection<DutyEvent> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<DutyEvent> Models { get; set; }
    }
}
