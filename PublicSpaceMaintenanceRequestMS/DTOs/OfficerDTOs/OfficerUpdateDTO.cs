using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs
{
    public class OfficerUpdateDTO
    {
        public UserDTO? UserDTO { get; set; }

        [StringLength(50, ErrorMessage = "Department name should not exceed 50 characters.")]
        public DepartmentPatchDTO? Department { get; set; }
    }
}
