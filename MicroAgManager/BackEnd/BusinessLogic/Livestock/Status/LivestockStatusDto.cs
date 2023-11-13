using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class LivestockStatusDto
    {
        public LivestockStatusDto(long count, ICollection<LivestockStatusModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<LivestockStatusModel> Models { get; set; }
    }
}
