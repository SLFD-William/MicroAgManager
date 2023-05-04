namespace Domain.Enums
{
    public class TenantAccessLevelEnum : BaseEnumeration
    {
        public static TenantAccessLevelEnum SingleUser = new(2, nameof(SingleUser));
        public static TenantAccessLevelEnum SingleUserWithAttachments = new(3, nameof(SingleUserWithAttachments));
        public static TenantAccessLevelEnum MultiUser = new(4, nameof(MultiUser));
        public static TenantAccessLevelEnum MultiUserWithAttachments = new(5, nameof(MultiUserWithAttachments));
        public static TenantAccessLevelEnum God = new(6, nameof(God));
        public TenantAccessLevelEnum(long id, string name) : base(id, name)
        {
        }
    }
}
