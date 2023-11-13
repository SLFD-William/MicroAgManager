using Domain.Models;

namespace BackEnd.BusinessLogic.Duty
{
    public class DutyDto
    {
        public DutyDto(long count, ICollection<DutyModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<DutyModel> Models { get; set; }


    }
}
