using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.Abstracts
{
    public abstract class BaseHasRecipientModel:BaseModel,IHasRecipient
    {
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
    }
}
