using PublicSpaceMaintenanceRequestMS.Data;

namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<User>> GetAllUsersAdminsAsync();
    }
}
