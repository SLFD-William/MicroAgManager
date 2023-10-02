using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RegistrationModel:BaseModel
    {
        [Required][ForeignKey("Registrar")] public long RegistrarId { get; set; }
        public virtual RegistrarModel Registrar { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
        [Required][MaxLength(40)] public string Identifier { get; set; }
        [Required] public bool DefaultIdentification { get; set; } = false;
        [Required] public DateTime RegistrationDate { get; set; }

        public static RegistrationModel Create(Registration? registration)
        {
            var model = PopulateBaseModel(registration, new RegistrationModel
            {
                RegistrarId = registration.RegistrarId,
                RecipientTypeId = registration.RecipientTypeId,
                RecipientType = registration.RecipientType,
                RecipientId = registration.RecipientId,
                Identifier = registration.Identifier,
                DefaultIdentification = registration.DefaultIdentification,
                Registrar = RegistrarModel.Create(registration.Registrar),
                RegistrationDate = registration.RegistrationDate
            }) as RegistrationModel;
            return model;
        }
        public Registration MapToEntity(Registration registration)
        {
            registration.RegistrarId = RegistrarId;
            registration.RecipientTypeId = RecipientTypeId;
            registration.RecipientType = RecipientType;
            registration.RecipientId = RecipientId;
            registration.Identifier = Identifier;
            registration.DefaultIdentification = DefaultIdentification;
            registration.ModifiedOn = DateTime.UtcNow;
            registration.RegistrationDate = RegistrationDate;
            return registration;
        }
    }
}
