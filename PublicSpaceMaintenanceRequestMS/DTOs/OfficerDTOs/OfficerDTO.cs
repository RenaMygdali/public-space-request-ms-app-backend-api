using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs
{
    public class OfficerDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "DepartmentId is required.")]
        public int DepartmentId { get; set; }

        public UserDTO? User { get; set; }
    }
}
