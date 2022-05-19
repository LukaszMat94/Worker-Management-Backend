using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Data.Context
{
    public class WorkersManagementDBContext : DbContext
    {
        public WorkersManagementDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies => Set<Company>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Technology> Technologies => Set<Technology>();
        public DbSet<Role> Roles => Set<Role>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Role

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            #endregion

            #region  Company

            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Company>()
                .Property(c => c.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Users)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region User

            modelBuilder.Entity<User>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<User>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(c => c.Surname)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<User>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Entity<User>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Email", "[Email] LIKE '%_@_%._%'");

            modelBuilder.Entity<User>()
                .Property(c => c.Password)
                .HasMaxLength(512);

            modelBuilder.Entity<User>()
                .HasOne(w => w.Role)
                .WithOne()
                .HasForeignKey<User>(w => w.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.RoleId)
                .IsUnique(false);

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

            #endregion

            #region Project

            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            #endregion
        }
    }
}
