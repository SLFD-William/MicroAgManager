namespace Domain.Interfaces
{
    public interface IBaseCommand
    {
        public Guid ModifiedBy { get; set; }
        public Guid TenantId { get; set; }
    }
}
