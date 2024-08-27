using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs
{
    public class UserFiltersDTO
    {
        public string? Username { get; set; }
        public UserRole? Role { get; set; }
    }
}
