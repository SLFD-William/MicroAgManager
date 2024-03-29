﻿using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RegistrationModel : BaseHasRecipientModel
    {
        [Required][ForeignKey("Registrar")] public long RegistrarId { get; set; }
        public virtual RegistrarModel Registrar { get; set; }
        [Required][MaxLength(40)] public string Identifier { get; set; }
        [Required] public bool DefaultIdentification { get; set; } = false;
        [Required] public DateTime RegistrationDate { get; set; }

        public static RegistrationModel Create(Registration? registration)
        {
            var model = PopulateBaseModel(registration, new RegistrationModel
            {
                RegistrarId = registration.RegistrarId,
                Identifier = registration.Identifier,
                DefaultIdentification = registration.DefaultIdentification,
                Registrar = RegistrarModel.Create(registration.Registrar),
                RegistrationDate = registration.RegistrationDate
            }) as RegistrationModel;
            return model;
        }

        public override BaseModel Map(BaseModel registration)
        {
            if (registration == null || registration is not RegistrationModel) return null;
            ((RegistrationModel)registration).RegistrarId = RegistrarId;
            ((RegistrationModel)registration).RecipientTypeId = RecipientTypeId;
            ((RegistrationModel)registration).RecipientType = RecipientType;
            ((RegistrationModel)registration).RecipientId = RecipientId;
            ((RegistrationModel)registration).Identifier = Identifier;
            ((RegistrationModel)registration).DefaultIdentification = DefaultIdentification;
            ((RegistrationModel)registration).RegistrationDate = RegistrationDate;
            return registration;
        }

        public override BaseEntity Map(BaseEntity registration)
        {
            if (registration == null || registration is not Registration) return null;
            ((Registration)registration).RegistrarId = RegistrarId;
            ((Registration)registration).RecipientTypeId = RecipientTypeId;
            ((Registration)registration).RecipientType = RecipientType;
            ((Registration)registration).RecipientId = RecipientId;
            ((Registration)registration).Identifier = Identifier;
            ((Registration)registration).DefaultIdentification = DefaultIdentification;
            registration.ModifiedOn = DateTime.UtcNow;
            ((Registration)registration).RegistrationDate = RegistrationDate;
            return registration;
        }
    }
}
