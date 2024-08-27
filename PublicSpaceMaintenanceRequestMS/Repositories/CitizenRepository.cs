using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class CitizenRepository : BaseRepository<Citizen>, ICitizenRepository
    {

        public CitizenRepository(PublicSpaceMaintenanceRequestDbDBContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Retrieves all users with the role of citizen from the database.
        /// Includes related citizen details for each user.
        /// </summary>
        /// <returns> 
        /// A task that represents the asynchronous operation. 
        /// The task result contains a list of users with citizen role.
        /// </returns>
        public async Task<List<User>> GetAllUsersCitizensAsync()
        {
            var usersWithCitizenRole = await _dbContext.Users
                .Where(u => u.Role == UserRole.Citizen)
                .Include(u => u.Citizen)
                .ToListAsync();

            return usersWithCitizenRole;
        }


        /// <summary>
        /// Retrives a citizen with the given user ID from the database.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the citizen with the specified user ID.
        /// </returns>
        public async Task<Citizen?> GetByUserIdAsync(int userId)
        {
            var citizen = await _dbContext.Citizens.FirstOrDefaultAsync(u => u.UserId == userId);
            return citizen;
        }
    }
}
