using Microsoft.EntityFrameworkCore;
using RAR;

namespace RarApiConsole.providers
{
    using dataObjects;
    internal class DatabaseContext : DbContext
    {
        public DbSet<DoUser> users { get; set; } = null!;
        public DbSet<DoTask> tasks { get; set; } = null!;
        public DbSet<DoScoreboard> scoreboards { get; set; } = null!;
        public DbSet<DoReward> rewards { get; set; } = null!;
        public DbSet<DoReferral> referrals { get; set; } = null!;
        public DbSet<DoProfile> profiles { get; set; } = null!;
        public DbSet<DoCandidate> candidates { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var s = TSettings.Instance();
            var conf = s.GetDataBase();

            string dbConnection = "User ID = postgres; Password = Fewnbd6g; Host = localhost; port = 5432; Database = Project C; Pooling = true";

            if (conf !=null)
            {
                 dbConnection = "User ID = " + conf.UserID + "; Password = " + conf.Password + "; Host = " + conf.Host + "; port = " + conf.Port + "; Database = " + conf.DataBase + "; Pooling = " + conf.Pooling + ";";
            }
           
            optionsBuilder.UseNpgsql(dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            // Candidate foreign keys
            modelBuilder.Entity<DoCandidate>()
                .HasOne(typeof(DoProfile))
                .WithMany()
                .HasForeignKey("fk_profile");

            // Referral foreign keys
            modelBuilder.Entity<DoReferral>()
                .HasOne(typeof(DoUser))
                .WithMany()
                .HasForeignKey("fk_user");

            modelBuilder.Entity<DoReferral>()
                .HasOne(typeof(DoTask))
                .WithMany()
                .HasForeignKey("fk_task");

            modelBuilder.Entity<DoReferral>()
                .HasOne(typeof(DoCandidate))
                .WithMany()
                .HasForeignKey("fk_candidate");

            modelBuilder.Entity<DoReferral>()
                .HasOne(typeof(DoScoreboard))
                .WithMany()
                .HasForeignKey("fk_scoreboard");

            // Reward foreign keys
            modelBuilder.Entity<DoReward>()
                .HasOne(typeof(DoUser))
                .WithMany()
                .HasForeignKey("fk_user");

            // Scoreboard foreign keys
            modelBuilder.Entity<DoScoreboard>()
                .HasOne(typeof(DoUser))
                .WithMany()
                .HasForeignKey("fk_user");

            // User foreign keys
            modelBuilder.Entity<DoUser>()
                .HasOne(typeof(DoProfile))
                .WithMany()
                .HasForeignKey("fk_profile");
        }
    }
}
