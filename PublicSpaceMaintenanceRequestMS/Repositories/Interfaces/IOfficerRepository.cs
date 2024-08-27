using PublicSpaceMaintenanceRequestMS.Data;

namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface IOfficerRepository
    {
        Task<IEnumerable<Officer>> GetByDepartment(int departmentId);
        Task<List<User>> GetAllUsersOfficersAsync();
        Task<Officer?> GetByUserIdAsync(int userId);
        Task<bool> AssignOfficerAsync(int officerId, int departmentId);
    }
}
