using PublicSpaceMaintenanceRequestMS.Data;

namespace PublicSpaceMaintenanceRequestMS.Services.Interfaces
{
    public interface ICitizenService
    {
        Task<Citizen?> GetCitizenByIdAsync(int id);
    }
}
