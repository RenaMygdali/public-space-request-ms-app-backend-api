using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs
{
    public class DepartmentPatchDTO
    {
        [Required(ErrorMessage = "Department title is required.")]
        [StringLength(50, ErrorMessage = "Department title cannot exceed 50 characters.")]
        public string? Title { get; set; }
    }
}
