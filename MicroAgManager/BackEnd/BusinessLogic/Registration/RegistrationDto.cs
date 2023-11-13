using Domain.Models;

namespace BackEnd.BusinessLogic.Registration
{
    public class RegistrationDto
    {
        public RegistrationDto(long count, ICollection<RegistrationModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<RegistrationModel> Models { get; set; }
    }
}
