using Domain.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Abstracts
{
    public abstract class BaseModel : ICloneable
    {
        [Required] public long Id { get; set; }
        [Required] public DateTime EntityModifiedOn { get; set; }
        [Required] public Guid ModifiedBy { get; set; }
        public bool Deleted { get; set; }

        public static BaseModel PopulateBaseModel(BaseEntity entity, BaseModel model)
        {
            model.Id = entity.Id;
            model.Deleted = entity.DeletedOn.HasValue;
            model.EntityModifiedOn = entity.ModifiedOn;
            model.ModifiedBy = entity.ModifiedBy;

            if (entity is IHasRecipient)
            {
                ((IHasRecipient)model).RecipientId= ((IHasRecipient)entity).RecipientId;
                ((IHasRecipient)model).RecipientType = ((IHasRecipient)entity).RecipientType;
                ((IHasRecipient)model).RecipientTypeId = ((IHasRecipient)entity).RecipientTypeId;
            }
            return model;
        }
        public abstract BaseModel Map(BaseModel model);
        public abstract BaseEntity Map(BaseEntity entity);
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static object ParseJsonString(string jsonString, Type type)
        {
            return JsonConvert.DeserializeObject(jsonString, type);
        }

        public static T ParseJsonString<T>(string jsonString) where T : BaseModel
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public virtual string GetEntityName()=>GetType().Name.Replace("Model", "");
    }
}
