using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class LivestockAnimalDto
    {
        public LivestockAnimalDto(long count, ICollection<LivestockAnimalModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<LivestockAnimalModel> Models { get; set; }
    }
}
