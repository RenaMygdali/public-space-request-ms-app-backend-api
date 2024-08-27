using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetByStatusAsync(RequestStatus status);
        Task<IEnumerable<Request>> GetByCitizenIdAsync(int citizenId);
        Task<IEnumerable<Request>> GetByDepartmentAsync(int departmentId);   
        Task<bool> UpdateRequestStatusAsync(int requestId, RequestStatus status);
        Task<bool> AssignRequestAsync(int requestId, int departmentId);
        Task<List<Request>> GetAllRequestsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Request, bool>>> predicates);
        Task<List<Request>> GetAllRequestsWithDetailsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Request, bool>>> predicates);
    }
}
