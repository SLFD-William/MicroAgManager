using Domain.Context;

namespace Domain.Interfaces
{
    public interface IFrontEndDbContextFactory
    {
        Task<FrontEndDbContext> CreateFrontEndDbContextAsync();
    }
}
