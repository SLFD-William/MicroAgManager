using Domain.Models;

namespace BackEnd.BusinessLogic.Unit
{
    public class UnitDto
    {
        public UnitDto(long count, ICollection<UnitModel> models)
        {
            Count = count;
            Models = models;
        }
        public long Count { get; set; }
        public ICollection<UnitModel> Models { get; set; }
    }
}
