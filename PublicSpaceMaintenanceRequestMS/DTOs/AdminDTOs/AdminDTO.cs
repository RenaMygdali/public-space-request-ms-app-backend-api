using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;

namespace PublicSpaceMaintenanceRequestMS.DTOs.AdminDTOs
{
    public class AdminDTO : UserDTO
    {
        public UserDTO? User { get; set; }
    }
}
