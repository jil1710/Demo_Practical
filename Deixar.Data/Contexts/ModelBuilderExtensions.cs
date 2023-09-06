using Deixar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deixar.Data.Contexts
{
    public static class ModelBuilderExtensions
    {
        //Data seeder for Users entity
        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User() { Id = 1, FirstName = "Bhavin", LastName = "Kareliya", EmailAddress = "bhavin@gmail.com", Password = "123123", ContactNumber = "1231231231", IsDeleted = false, CreatedAt = new DateTime(2023, 8, 1)},
                    new User() { Id = 2, FirstName = "Jil", LastName = "Patel", EmailAddress = "jil@gmail.com", Password = "123123", ContactNumber = "1231231231", IsDeleted = true, CreatedAt = new DateTime(2023, 8, 1) },
                    new User() { Id = 3, FirstName = "Vipul", LastName = "Kumar", EmailAddress = "vipul@gmail.com", Password = "123123", ContactNumber = "1231231231", IsDeleted = false, CreatedAt = new DateTime(2023, 8, 1) }
                );
        }

        //Data seeder for Roles entity
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                    new Role() { Id = 1, RoleName = "HR" },
                    new Role() { Id = 2, RoleName = "User" }
                );
        }

        //Data seeder for UserRoles entity
        public static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                    new UserRole() { RoleId = 1, UserId = 1 },
                    new UserRole() { RoleId = 2, UserId = 2 },
                    new UserRole() { RoleId = 2, UserId = 3 }
                );
        }
    }
}
