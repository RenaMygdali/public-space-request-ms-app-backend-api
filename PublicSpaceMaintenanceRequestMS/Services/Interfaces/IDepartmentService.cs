using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;

namespace PublicSpaceMaintenanceRequestMS.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<Department> AddDepartmentAsync(DepartmentCreateDTO departmentCreateDTO);
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<List<DepartmentDTO>> GetAllDepartmentsFilteredAsync(int pageNumber, int pageSize,
             DepartmentFiltersDTO departmentFiltersDTO);
        Task<List<DepartmentDTO>> GetAllDepartmentsAsync();
        Task DeleteDepartmentAsync(int id);
        Task UpdateDepartmentTitleAsync(int id, string newTitle);
    }
}
