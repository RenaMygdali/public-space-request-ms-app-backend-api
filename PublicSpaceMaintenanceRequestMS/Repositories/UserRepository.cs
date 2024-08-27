using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Security;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        private readonly IMapper _mapper;

        public UserRepository(PublicSpaceMaintenanceRequestDbDBContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves users from the database asynchronously based on their lastname.
        /// </summary>
        /// <param name="lastname">The lastname of the users to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of users with the specified lastname.</returns>
        public async Task<IEnumerable<User>> GetByLastnameAsync(string lastname)
        {
            var usersByLastname = await _dbContext.Users
                .Where(u => u.Lastname == lastname)
                .ToListAsync();

            return usersByLastname;
        }

        /// <summary>
        /// Retrieves users from the database asynchronously based on their role.
        /// </summary>
        /// <param name="role">The role of the users to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of users with the specified role.</returns>
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            var usersByRole = await _dbContext.Users
                .Where(u => u.Role == role)
                .ToListAsync();

            return usersByRole;
        }


        /// <summary>
        /// Retrieves a user from the database asynchronously based on its username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user with the specified username, or null if not found.</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }


        /// <summary>
        /// Retrieves a user from the database asynchronously based on its email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user with the specified email, or null if not found.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }


        /// <summary>
        /// Retrieves a user from the database asynchronously based on username or email and verifies the password.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user to retrieve.</param>
        /// <param name="password">The password to verify against the retrieved user's password.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if authentication is successful, or null if not found or authentication fails.</returns>
        public async Task<User?> GetByCredentialsAsync(string usernameOrEmail, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail
                    || u.Email == usernameOrEmail);

            if (user == null) return null;

            if (!EncryptionUtil.IsValidPassword(password, user.Password!))
            {
                return null;
            }

            return user;
        }


        /// <summary>
        /// Asynchronously updates a user in the database.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="user">The entity to update with.</param>
        /// <returns>The existing user object if the update is successful, otherwise <c>null</c> if the user is not found.</returns>
        /// <remarks>
        /// This method first checks if a user with the specified <paramref name="userId"/> exists in the database.
        /// If the user is found, it updates the user object. If the user is not found, it returns <c>null</c>.
        /// </remarks>
        public async Task<User?> UpdateUserAsync(int userId, User user)
        {
            // Optimized Query - First Where, then FirstAsync
            var existingUser = await _dbContext.Users.Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            // Not optimized - just searches
            //var user = await _context.Users.FirstAsync(x => x.Id == userId);

            if (existingUser is null) return null;

            //_dbContext.Users.Attach(user);
            //_dbContext.Entry(user).State = EntityState.Modified;

            _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            _dbContext.Entry(existingUser).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingUser;
        }


        /// <summary>
        /// Sets the role of a user in the database asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose role will be updated.</param>
        /// <param name="role">The new role to assign to the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the role was successfully updated, false otherwise (e.g., if the user with the given ID does not exist).</returns>
        public async Task<bool> UpdateUserRoleAsync(int userId, UserRole role)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user is null) return false;

            user.Role = role;

            return true;
        }

        public async Task<List<User>> GetAllUsersFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            int skip = pageSize * (pageNumber - 1);
            IQueryable<User> query = _dbContext.Users.AsQueryable();

            // Combine multiple predicates using && operator
            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }
    }
}
