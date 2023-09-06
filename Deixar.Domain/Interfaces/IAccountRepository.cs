using Deixar.Domain.DTOs;

namespace Deixar.Domain.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get user information by email address and password
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="password">The email Password</param>
        /// <returns>User object</returns>
        Task<UserDetails> GetUserByEmailPasswordAsync(string email, string password);

        /// <summary>
        /// Check if user exist or not
        /// </summary>
        /// <param name="email">User email address</param>
        /// <returns></returns>
        Task<bool> IsUserExist(string email);

        /// <summary>
        /// Return all user roles by comma sepereted string (HR, User)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> GetUserRoles(string email);

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> RegisterUserAsync(RegisterUserModel user);
    }
}
