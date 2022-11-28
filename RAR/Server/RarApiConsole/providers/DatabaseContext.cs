using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.providers
{
    using dataObjects;
    internal class DatabaseContext : DbContext
    {
        public DbSet<DoUser> ?users { get; set; }
        public DbSet<DoTask>? tasks { get; set; }
        public DbSet<DoScoreboard>? scoreboards { get; set; }
        public DbSet<DoReward>? rewards { get; set; }
        public DbSet<DoReferral>? referrals { get; set; }
        public DbSet<DoProfile>? profiles { get; set; }
        public DbSet<DoCandidate>? candidates { get; set; }
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
