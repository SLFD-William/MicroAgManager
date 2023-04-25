using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Server.Abstracts
{
    public abstract class BaseQuery
    {
#nullable enable
        public long? Id { get; set; }
#nullable disable
        [Required] public Guid TenantId { get; set; }
        public BaseModel NewModel { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public DateTime? LastModified { get; set; }

        //public abstract Task<BaseModel> GetModel(CancellationToken cancellationToken);
        //public abstract Task<Tuple<long, ICollection<BaseModel>>> GetModels(CancellationToken cancellationToken);

    }
}
