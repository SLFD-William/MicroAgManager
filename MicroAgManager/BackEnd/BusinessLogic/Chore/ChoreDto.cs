using Domain.Models;

namespace BackEnd.BusinessLogic.Chore
{
    public class ChoreDto
    {
        public ChoreDto(long count, ICollection<ChoreModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<ChoreModel> Models { get; set; }
    }
}
