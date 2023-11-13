using Domain.Models;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class BreedingRecordDto
    {
        public BreedingRecordDto(long count, ICollection<BreedingRecordModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<BreedingRecordModel> Models { get; set; }
    }
}
