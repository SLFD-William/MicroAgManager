using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class LivestockBreedDto
    {
        public LivestockBreedDto(long count, ICollection<LivestockBreedModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<LivestockBreedModel> Models { get; set; }
    }
}
