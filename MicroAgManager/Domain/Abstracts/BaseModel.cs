using System.ComponentModel.DataAnnotations;

namespace Domain.Abstracts
{
    public abstract class BaseModel
    {
        [Required]
        public long Id { get; set; }
        public bool Deleted { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }
        [Required]
        public DateTime EntityModifiedOn { get; set; } = DateTime.MinValue;
        [Required]
        public Guid ModifiedBy { get; set; }

        


        public static BaseModel PopulateBaseModel(BaseEntity entity, BaseModel model)
        {
            model.Id = entity.Id;
            model.Deleted = entity.DeletedOn.HasValue;
            model.ModifiedOn = entity.ModifiedOn;
            model.EntityModifiedOn = entity.ModifiedOn;
            model.ModifiedBy = entity.ModifiedBy;
            return model;
        }
    }
}
