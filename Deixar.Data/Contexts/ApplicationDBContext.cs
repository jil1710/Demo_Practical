using Deixar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deixar.Data.Contexts
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) { }

        //Register Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure entities
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<User>().HasIndex(u => u.EmailAddress).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(u => u.RoleName).IsUnique();
            modelBuilder.Entity<Leave>().HasOne(l => l.CreatedByUser).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Leave>().HasOne(l => l.StatusUpdatedByUser).WithMany().OnDelete(DeleteBehavior.Restrict);

            //Seed database entities
            modelBuilder.SeedUsers();
            modelBuilder.SeedRoles();
            modelBuilder.SeedUserRoles();
        }
    }
}
