using Microsoft.EntityFrameworkCore;
using AbySalto.Junior.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AbySalto.Junior.Infrastructure.Database
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Item> Items { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
