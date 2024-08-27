using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;

namespace PublicSpaceMaintenanceRequestMS.Services.Interfaces
{
    public interface IRequestService
    {
        Task<Request> AddRequestAsync(RequestSubmitDTO requestSubmitDTO, int citizenId);
        Task<List<RequestDTO>> GetAllRequestsAsync();
        Task<List<RequestDTO>> GetAllRequestsFilteredAsync(int pageNumber, int pageSize,
             RequestFiltersDTO requestFiltersDTO);
        Task<List<RequestDTO>> GetAllRequestsWithDetailsFilteredAsync(int pageNumber, int pageSize,
            RequestFiltersDTO requestFiltersDTO);
        Task<List<RequestDTO>> GetUserRequestsFilteredAsync(int citizenId, int pageNumber, int pageSize, RequestFiltersDTO requestFiltersDTO);
        Task<bool> AssignRequestToDepartmentAsync(int requestId, int departmentId);
    }
}
