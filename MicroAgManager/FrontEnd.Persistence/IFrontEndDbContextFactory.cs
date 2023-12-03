namespace FrontEnd.Persistence
{
    public interface IFrontEndDbContextFactory
    {
        Task<FrontEndDbContext> CreateFrontEndDbContextAsync();
    }
}
