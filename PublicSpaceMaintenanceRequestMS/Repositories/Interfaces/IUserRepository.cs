using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByCredentialsAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByLastnameAsync(string lastname);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<List<User>> GetAllUsersFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates);
        Task<User?> UpdateUserAsync(int userId, User user);
        Task<bool> UpdateUserRoleAsync(int userId, UserRole role);
    }
}
