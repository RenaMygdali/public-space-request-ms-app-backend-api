using PublicSpaceMaintenanceRequestMS.DTOs.CitizenDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;
using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs
{
    public class RequestReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public CitizenDTO? Citizen { get; set; }
        public DepartmentDTO? AssignedDepartment { get; set; }
    }
}
