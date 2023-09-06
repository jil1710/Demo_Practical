using Deixar.Data.Contexts;
using Deixar.Domain.DTOs;
using Deixar.Domain.Entities;
using Deixar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Deixar.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        readonly ApplicationDBContext _db;

        public AccountRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<UserDetails> GetUserByEmailPasswordAsync(string email, string password)
        {
            User user = await _db.Users.SingleOrDefaultAsync<User>(u => u.EmailAddress == email && u.Password == password && !u.IsDeleted);
            string role = await GetUserRoles(user.EmailAddress);
            UserDetails userDetails = new()
            {
                User = user,
                Role = role
            };
            return userDetails;
        }

        public async Task<bool> IsUserExist(string email)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.EmailAddress == email && !u.IsDeleted);
            return user is not null;
        }

        public async Task<string> GetUserRoles(string email)
        {
            int userId = _db.Users.FirstAsync(u => u.EmailAddress == email).Result.Id;
            var roles = await _db.UserRoles.Where(ur => ur.UserId == userId)
                .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.RoleId, r.RoleName })
                .Select(ur => ur.RoleName).ToArrayAsync();
            return String.Join(", ", roles);
        }

        public async Task<int> RegisterUserAsync(RegisterUserModel user)
        {
            try
            {
                await _db.Users.AddAsync(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    Password = user.Password,
                    ContactNumber = user.ContactNumber,
                    Address = user.Address,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    MiddleName = user.MiddleName
                });
                await _db.SaveChangesAsync();
                int id = _db.Users.Max(u => u.Id);
                int roleId = _db.Roles.SingleAsync(r => r.RoleName == user.Role).Result.Id;
                await _db.UserRoles.AddAsync(new UserRole() { RoleId = roleId, UserId = id });
                await _db.SaveChangesAsync();
                return id;
            }
            catch
            {
                return -1;
            }
        }
    }
}
