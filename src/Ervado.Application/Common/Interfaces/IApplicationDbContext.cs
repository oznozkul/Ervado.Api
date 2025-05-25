using Ervado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<ProductCategory> ProductCategories { get; }
        DbSet<Brand> Brands { get; }
        DbSet<Model> Models { get; }
        DbSet<Inventory> Inventories { get; }
        DbSet<StockMovement> StockMovements { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 