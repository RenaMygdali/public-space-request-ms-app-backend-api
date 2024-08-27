using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs
{
    public class OfficerAssignDTO
    {
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Department Id is required.")]
        public int DepartmentId { get; set; }
    }
}
