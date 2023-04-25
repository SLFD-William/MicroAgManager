using System.ComponentModel.DataAnnotations;

namespace Domain.Abstracts
{
    public class BaseModel
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
        public static BaseModel Create(object dro) => throw new NotImplementedException(dro.ToString());

        public static BaseModel PopulateBaseModel(BaseEntity entity, BaseModel model)
        {
            model.Id = entity.Id;
            model.Deleted = entity.Deleted.HasValue;
            model.ModifiedOn = entity.ModifiedOn;
            model.EntityModifiedOn = entity.ModifiedOn;
            model.ModifiedBy = entity.ModifiedBy;
            return model;
        }
    }
}
