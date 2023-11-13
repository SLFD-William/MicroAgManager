using Domain.Models;

namespace BackEnd.BusinessLogic.Measurement
{
    public class MeasurementDto
    {
        public MeasurementDto(long count, ICollection<MeasurementModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<MeasurementModel> Models { get; set; }
    }
}
