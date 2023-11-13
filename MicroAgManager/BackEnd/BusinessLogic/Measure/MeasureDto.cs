using Domain.Models;

namespace BackEnd.BusinessLogic.Measure
{
    public class MeasureDto
    {
        public MeasureDto(long count, ICollection<MeasureModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<MeasureModel> Models { get; set; }
    }
}
