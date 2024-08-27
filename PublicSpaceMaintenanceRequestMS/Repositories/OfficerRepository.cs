using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class OfficerRepository : BaseRepository<Officer>, IOfficerRepository
    {

        public OfficerRepository(PublicSpaceMaintenanceRequestDbDBContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Retrieves all users with the role of officer from the database.
        /// Includes related officer details for each user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of users with officer role.</returns>
        public async Task<List<User>> GetAllUsersOfficersAsync()
        {
            var usersWithOfficerRole = await _dbContext.Users
                .Where(u => u.Role == UserRole.Officer)
                .Include(u => u.Officer)
                .ToListAsync();

            return usersWithOfficerRole;
        }


        /// <summary>
        /// Retrieves all officers belonging to a specified department from the database.
        /// </summary>
        /// <param name="departmentId">The ID of the department to filter officers by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of officers belonging to the specified department.</returns>
        public async Task<IEnumerable<Officer>> GetByDepartment(int departmentId)
        {
            var officersByDepartment = await _dbContext.Officers
                .Where(o => o.DepartmentId == departmentId)
                .ToListAsync();

            return officersByDepartment;
        }

        /// <summary>
        /// Retrives an officer with the given user ID from the database.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the officer with the specified user ID.
        /// </returns>
        public async Task<Officer?> GetByUserIdAsync(int userId)
        {
            var officer = await _dbContext.Officers.FirstOrDefaultAsync(u => u.UserId == userId);
            return officer;
        }

        /// <summary>
        /// Assigns an officer with the given ID to a department.
        /// </summary>
        /// <param name="officerId">The ID of the officer to be assigned.</param>
        /// <param name="departmentId">The ID of the department to assign to.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result indicates whether the assignment was successful (true) or not (false).
        /// </returns>
        public async Task<bool> AssignOfficerAsync(int officerId, int departmentId)
        {
            var officer = await _dbContext.Officers.FindAsync(officerId);

            if (officer == null) return false;

            officer.DepartmentId = departmentId;
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
