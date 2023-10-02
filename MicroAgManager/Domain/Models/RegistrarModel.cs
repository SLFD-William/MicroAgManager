using Domain.Abstracts;
using Domain.Entity;

namespace Domain.Models
{
    public class RegistrarModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string API { get; set; } = string.Empty;
        public virtual ICollection<RegistrationModel> Registrations { get; set; } = new List<RegistrationModel>();
        public static RegistrarModel Create(Registrar registrar)
        {
            var model = PopulateBaseModel(registrar, new RegistrarModel
            {
                Name = registrar.Name,
                Email = registrar.Email,
                Website = registrar.Website,
                API = registrar.API,
                Registrations = registrar.Registrations?.Select(RegistrationModel.Create).ToList() ?? new List<RegistrationModel>(),
            }) as RegistrarModel;
            return model;
        }
        public Registrar MapToEntity(Registrar registrar)
        {
            registrar.Name = Name;
            registrar.Email = Email;
            registrar.Website = Website;
            registrar.API = API;

            registrar.ModifiedOn = DateTime.UtcNow;
            return registrar;
        }
    }
}
