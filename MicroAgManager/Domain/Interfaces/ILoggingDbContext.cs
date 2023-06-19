using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface ILoggingDbContext
    {
        DbSet<Log> Logs { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
