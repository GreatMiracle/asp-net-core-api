using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;

namespace WebApplication1.Infrastructure.Data
{
    public class WalksDbContext : DbContext
    {
        public WalksDbContext(DbContextOptions options) : base(options)
        {

        }

        // Định nghĩa các DbSet cho các thực thể
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

    }
}
