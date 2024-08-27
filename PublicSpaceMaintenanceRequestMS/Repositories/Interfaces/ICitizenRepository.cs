using PublicSpaceMaintenanceRequestMS.Data;

namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface ICitizenRepository
    {
        Task<List<User>> GetAllUsersCitizensAsync();
        Task<Citizen?> GetByUserIdAsync(int userId);
    }
}
