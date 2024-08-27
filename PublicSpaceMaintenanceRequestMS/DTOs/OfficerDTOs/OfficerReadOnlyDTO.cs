using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;

namespace PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs
{
    public class OfficerReadOnlyDTO
    {
        public int Id { get; set; }
        public UserReadOnlyDTO? User { get; set; }
        public DepartmentReadOnlyDTO? Department { get; set; }
    }
}
