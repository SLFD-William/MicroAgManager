using System.ComponentModel.DataAnnotations;

namespace Domain.Abstracts
{
    public abstract class BaseModel
    {
        [Required] public long Id { get; set; }
        [Required] public DateTime EntityModifiedOn { get; private set; } = DateTime.MinValue;
        [Required] public Guid ModifiedBy { get; set; }
        public bool Deleted { get; set; }

        public static BaseModel PopulateBaseModel(BaseEntity entity, BaseModel model)
        {
            model.Id = entity.Id;
            model.Deleted = entity.DeletedOn.HasValue;
            model.EntityModifiedOn = entity.ModifiedOn;
            model.ModifiedBy = entity.ModifiedBy;
            return model;
        }

        public virtual string GetEntityName()=>GetType().Name.Replace("Model", "");
    }
}
