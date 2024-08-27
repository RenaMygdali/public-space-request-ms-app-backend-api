using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {

        // private readonly IMapper _mapper;


        public RequestRepository(PublicSpaceMaintenanceRequestDbDBContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Assigns a request to a specified department in the database.
        /// </summary>
        /// <param name="requestId">The ID of the request to assign.</param>
        /// <param name="departmentId">The ID of the department to assign the request to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the assignment was successful (true) or the request was not found (false).</returns>
        public async Task<bool> AssignRequestAsync(int requestId, int departmentId)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);

            if (request == null) return false;

            request.AssignedDepartmentId = departmentId;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves all requests associated with a specific citizen ID from the database.
        /// </summary>
        /// <param name="citizenId">The ID of the citizen to retrieve requests for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of requests associated with the specified citizen ID.</returns>
        public async Task<IEnumerable<Request>> GetByCitizenIdAsync(int citizenId)
        {
            return await _dbContext.Requests
                .Where(r => r.CitizenId == citizenId)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves all requests associated with a spesific department ID from the database.
        /// </summary>
        /// <param name="departmentId">The ID of the department to retrieve requests for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of requests associated with the specific department ID.</returns>
        public async Task<IEnumerable<Request>> GetByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Requests
                .Where(r => r.AssignedDepartmentId == departmentId)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves all requests with a specific status from the database.
        /// </summary>
        /// <param name="status">The status of the requests to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of requests with the specified status.</returns>
        public async Task<IEnumerable<Request>> GetByStatusAsync(RequestStatus status)
        {
            return await _dbContext.Requests
                .Where(r => r.Status == status)
                .ToListAsync();
        }


        /// <summary>
        /// Sets the status of a request identified by its ID in the database.
        /// </summary>
        /// <param name="requestId">The ID of the request to update.</param>
        /// <param name="status">The new status to set for the request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the status update was successful (true) or the request was not found (false).</returns>
        public async Task<bool> UpdateRequestStatusAsync(int requestId, RequestStatus status)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);

            if (request == null) return false;

            request.Status = status;

            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<List<Request>> GetAllRequestsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Request, bool>>> predicates)
        {
            int skip = pageSize * (pageNumber - 1);
            IQueryable<Request> query = _dbContext.Requests.AsQueryable();

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

        public async Task<List<Request>> GetAllRequestsWithDetailsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Request, bool>>> predicates)
        {
            int skip = pageSize * (pageNumber - 1);
            IQueryable<Request> query = _dbContext.Requests
                .Include(r => r.Citizen)
                    .ThenInclude(c => c!.User)
                .Include(r => r.AssignedDepartment)
                .AsQueryable();

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
