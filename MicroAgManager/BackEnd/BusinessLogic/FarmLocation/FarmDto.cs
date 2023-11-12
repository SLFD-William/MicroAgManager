using Domain.Models;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class FarmDto
    {
        public FarmDto(long count, ICollection<FarmLocationModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<FarmLocationModel> Models { get; set; }
    }
}
