using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs
{
    public class RequestAssignDTO
    {
        [Required(ErrorMessage = "Request Id is required.")]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Department Id is required.")]
        public int DepartmentId { get; set; }
    }
}
