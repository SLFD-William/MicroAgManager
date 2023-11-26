namespace Domain.ValueObjects
{
    public class UserInfo : ValueObject
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string TenantId { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return Email;
            yield return TenantId;
        }
    }
}
