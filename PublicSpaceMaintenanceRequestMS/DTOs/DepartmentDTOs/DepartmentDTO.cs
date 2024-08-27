using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department title is required.")]
        [StringLength(50, ErrorMessage = "Department title cannot exceed 50 characters.")]
        public string? Title { get; set; }

        // List of OfficerDTOs to represent the officers in the department
        public List<OfficerDTO>? Officers { get; set; }

        // List of RequestDTOs to represent the requests assigned to the department
        public List<RequestDTO>? Requests { get; set; }
    }
}
