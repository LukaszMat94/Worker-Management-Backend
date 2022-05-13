using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Entities;

namespace WorkerManagementAPI.Context
{
    public class WorkersManagementDBContext : DbContext
    {
        public WorkersManagementDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Worker> Workers => Set<Worker>();

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Technology> Technologies => Set<Technology>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region  Company
            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Company>()
                .Property(c => c.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Workers)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region Worker
            modelBuilder.Entity<Worker>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<Worker>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Worker>()
                .Property(c => c.Surname)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Worker>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Entity<Worker>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Worker>()
                .HasCheckConstraint("CK_Worker_Email", "[Email] LIKE '%_@_%._%'");

            modelBuilder.Entity<Worker>()
                .Property(c => c.Password)
                .HasMaxLength(70);

            #endregion

            #region Technology

            modelBuilder.Entity<Technology>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Technology>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Technology>()
                .Property(c => c.TechnologyLevel)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Technology>()
                .HasMany(c => c.Workers)
                .WithMany(c => c.Technologies)
                    .UsingEntity<Dictionary<string, object>>("WorkersTechnologies",
                        b => b.HasOne<Worker>()
                            .WithMany()
                            .HasForeignKey("WorkerId")
                            .OnDelete(DeleteBehavior.Restrict),
                        b => b.HasOne<Technology>()
                            .WithMany()
                            .HasForeignKey("TechnologyId")
                            .OnDelete(DeleteBehavior.Restrict));

            #endregion

            #region Project

            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>("ProjectsMembers",
                    mp => mp.HasOne<Worker>()
                        .WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Restrict),
                    mp => mp
                        .HasOne<Project>()
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Technologies)
                .WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>("ProjectsTechnologies",
                    mp => mp.HasOne<Technology>()
                        .WithMany()
                        .HasForeignKey("TechnologyId"),
                    mp => mp.HasOne<Project>()
                        .WithMany()
                        .HasForeignKey("ProjectId"));

            #endregion
        }
    }
}
