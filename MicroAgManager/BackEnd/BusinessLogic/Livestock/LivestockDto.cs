using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock
{
    public class LivestockDto
    {
        public LivestockDto(long count, ICollection<LivestockModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<LivestockModel> Models { get; set; }
    }
}
