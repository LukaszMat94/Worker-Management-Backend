using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;
using WorkerManagementAPI.Data.JwtToken;

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
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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
                .HasCheckConstraint("CK_Company_Name", "(LEN([Name]) > 2)")
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
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Name", "(LEN([Name]) > 3)")
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Surname)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Email", "[Email] LIKE '%_@_%._%'");

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(512);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.RoleId)
                .IsUnique(false);

            modelBuilder.Entity<User>()
                .Property(u => u.AccountStatus)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue(AccountStatusEnum.INACTIVE);

            modelBuilder.Entity<User>()
                .HasOne(u => u.RefreshToken)
                .WithOne(r => r.User)
                .HasForeignKey<RefreshToken>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Technology

            modelBuilder.Entity<Technology>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Technology>()
                .HasCheckConstraint("CK_Technology_Name", "(LEN([Name]) > 0)")
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Technology>()
                .Property(t => t.TechnologyLevel)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Technology>()
                .HasMany(t => t.Projects)
                .WithMany(p => p.Technologies)
                .UsingEntity<TechnologyProject>(
                    "TechnologiesProjects",
                    tp => tp
                        .HasOne(tp => tp.Project)
                        .WithMany()
                        .HasForeignKey(tp => tp.ProjectId),
                    tp => tp
                        .HasOne(tp => tp.Technology)
                        .WithMany()
                        .HasForeignKey(tp => tp.TechnologyId),
                    tp =>
                    {
                        tp.HasKey(tp => new { tp.ProjectId, tp.TechnologyId });
                    });

            modelBuilder.Entity<Technology>()
                .HasMany(t => t.Users)
                .WithMany(t => t.Technologies)
                .UsingEntity<UserTechnology>(
                    "UsersTechnologies",
                    ut => ut
                        .HasOne(ut => ut.User)
                        .WithMany()
                        .HasForeignKey(ut => ut.UserId),
                    ut => ut
                        .HasOne(ut => ut.Technology)
                        .WithMany()
                        .HasForeignKey(ut => ut.TechnologyId),
                    ut =>
                    {
                        ut.HasKey(ut => new { ut.UserId, ut.TechnologyId });
                    });

            #endregion

            #region Project

            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .HasCheckConstraint("CK_Project_Name", "(LEN([Name]) > 0)")
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Users)
                .WithMany(u => u.Projects)
                .UsingEntity<UserProject>(
                    "UsersProjects",
                    up => up
                        .HasOne(up => up.User)
                        .WithMany()
                        .HasForeignKey(up => up.UserId),
                    up => up
                        .HasOne(up => up.Project)
                        .WithMany()
                        .HasForeignKey(up => up.ProjectId),
                    up =>
                    {
                        up.HasKey(up => new { up.UserId, up.ProjectId });
                    });

            #endregion

            #region RefreshToken

            modelBuilder.Entity<RefreshToken>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<RefreshToken>()
                .Property(t => t.Token)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<RefreshToken>()
                .Property(t => t.Created)
                .IsRequired();

            modelBuilder.Entity<RefreshToken>()
                .Property(t => t.Expires)
                .IsRequired();

            modelBuilder.Entity<RefreshToken>()
                .Property(t => t.TokenStatus)
                .HasDefaultValue(true)
                .IsRequired();

            #endregion
        }
    }
}
