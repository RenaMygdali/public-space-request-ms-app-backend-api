namespace PublicSpaceMaintenanceRequestMS.Services.Interfaces
{
    public interface IOfficerService
    {
        Task<bool> AssignOfficerToDepartmentAsync(int officerId, int departmentId);
    }
}
