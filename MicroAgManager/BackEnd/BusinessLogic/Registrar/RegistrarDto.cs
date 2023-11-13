using Domain.Models;

namespace BackEnd.BusinessLogic.Registrar
{
    public class RegistrarDto
    {
        public RegistrarDto(long count, ICollection<RegistrarModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<RegistrarModel> Models { get; set; }
    }
}
