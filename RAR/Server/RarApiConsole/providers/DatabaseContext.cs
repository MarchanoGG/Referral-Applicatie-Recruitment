using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.providers
{
    using dataObjects;
    internal class DatabaseContext : DbContext
    {
        public DbSet<DoUser> ?users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID = postgres; Password = Fewnbd6g; Host = localhost; port = 5432; Database = Project C; Pooling = true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
        }

    }
}
