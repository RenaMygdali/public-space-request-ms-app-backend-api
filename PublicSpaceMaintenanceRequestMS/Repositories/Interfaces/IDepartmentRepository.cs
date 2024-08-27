using PublicSpaceMaintenanceRequestMS.Data;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByTitleAsync(string title);
        Task<List<Department>> GetAllDepartmentsFilteredAsync(
            int pageNumber, int pageSize, List<Expression<Func<Department, bool>>> predicates);
    }
}
