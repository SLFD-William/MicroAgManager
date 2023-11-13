using Domain.Models;

namespace BackEnd.BusinessLogic.TreatmentRecord
{
    public class TreatmentRecordDto
    {
        public TreatmentRecordDto(long count, ICollection<TreatmentRecordModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<TreatmentRecordModel> Models { get; set; }
    }
}
