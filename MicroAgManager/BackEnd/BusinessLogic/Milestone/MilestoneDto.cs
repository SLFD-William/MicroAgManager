using Domain.Models;

namespace BackEnd.BusinessLogic.Milestone
{
    public class MilestoneDto
    {
        public MilestoneDto(long count, ICollection<MilestoneModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<MilestoneModel> Models { get; set; }
    }
}
