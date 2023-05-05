using Domain.Enums;

namespace Domain.Entity
{
    public class Tenant
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public DateTime Created { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public Guid? DeletedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TenantUserAdminId { get; set; }
        public string AccessLevel { get; set; }
        public Tenant(Guid createdBy)
        {
            Created = DateTime.Now;
            CreatedBy = createdBy;
            ModifiedOn = Created;
            ModifiedBy = createdBy;
            AccessLevel = TenantAccessLevelEnum.SingleUser.GetDescription();
        }
    }
}
