namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key, Guid tenantId)
            : base($"Entity \"{name}\" ({key}) for TenantId {tenantId} was not found.")
        {
        }
    }
}