using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {

        public AdminRepository(PublicSpaceMaintenanceRequestDbDBContext context) :base(context)
        {
        }

        /// <summary>
        /// Retrieves all users with the role of admin from the database.
        /// Includes related admin details for each user.
        /// </summary>
        /// <returns> 
        /// A task that represents the asynchronous operation. 
        /// The task result contains a list of users with admin role.
        /// </returns>
        public async Task<List<User>> GetAllUsersAdminsAsync()
        {
            var usersWithAdminRole = await _dbContext.Users
                .Where(u => u.Role == UserRole.Admin)
                .Include(u => u.Admin)
                .ToListAsync();

            return usersWithAdminRole;
        }
    }
}
