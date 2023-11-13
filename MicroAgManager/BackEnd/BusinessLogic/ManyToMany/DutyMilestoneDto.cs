using Domain.ValueObjects;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class DutyMilestoneDto
    {
        public DutyMilestoneDto(long count, ICollection<DutyMilestone> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<DutyMilestone> Models { get; set; }
    }
}
