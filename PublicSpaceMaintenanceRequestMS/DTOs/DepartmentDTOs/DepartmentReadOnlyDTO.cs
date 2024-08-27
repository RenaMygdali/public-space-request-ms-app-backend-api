using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;

namespace PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs
{
    public class DepartmentReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<OfficerReadOnlyDTO>? Officers { get; set; }
        public List<RequestReadOnlyDTO>? Requests { get; set; }
    }
}
