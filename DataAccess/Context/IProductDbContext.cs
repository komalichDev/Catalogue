using System.Threading;
using System.Threading.Tasks;
using DatabaseAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Context;
public interface IProductDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Description> Descriptions { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}