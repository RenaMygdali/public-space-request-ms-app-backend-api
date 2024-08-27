using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;

namespace PublicSpaceMaintenanceRequestMS.DTOs.CitizenDTOs
{
    public class CitizenDTO : UserDTO
    {
        public UserDTO? User { get; set; }
    }
}
