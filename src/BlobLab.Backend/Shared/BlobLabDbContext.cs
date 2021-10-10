
using Microsoft.EntityFrameworkCore;

namespace BlobLab.Backend.Shared
{
    public class BlobLabDbContext : DbContext
    {
        public BlobLabDbContext(DbContextOptions<BlobLabDbContext> options) : base(options) { }

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<File>().HasKey(e => e.Id);
        }
    }
}
