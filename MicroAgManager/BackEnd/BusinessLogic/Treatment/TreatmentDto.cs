using Domain.Models;

namespace BackEnd.BusinessLogic.Treatment
{
    public class TreatmentDto
    {
        public TreatmentDto(long count, ICollection<TreatmentModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<TreatmentModel> Models { get; set; }
    }
}
