namespace Domain.Constants
{
    public static class Administration
    {

        public static Guid SystemAdminRole { get; private set; } = Guid.Parse("CD10345B-BC5D-4CE6-0E98-08D76B81D46D");
        public static Guid TenantAdminRole { get; private set; } = Guid.Parse("CD10345B-BC5D-4CE6-0E98-08D76B81D46E");
    }
}
