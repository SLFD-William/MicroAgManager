using Domain.ValueObjects;

namespace BackEnd.BusinessLogic.ManyToMany
{
    public class DutyChoreDto
    {
        public DutyChoreDto(long count, ICollection<DutyChore> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<DutyChore> Models { get; set; }
    }
}
