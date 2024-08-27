using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs
{
    public class DepartmentCreateDTO
    {
        [Required(ErrorMessage = "Department title is required.")]
        [StringLength(50, ErrorMessage = "Department title cannot exceed 50 characters.")]
        public string? Title { get; set; }

        public List<OfficerDTO>? Officers { get; set; }

        public List<RequestDTO>? Requests { get; set; }
    }
}
