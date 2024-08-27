using PublicSpaceMaintenanceRequestMS.DTOs.AdminDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.CitizenDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public string? Username { get; set; }

        [StringLength(50, ErrorMessage = "Email should not exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? Password { get; set; }

        [StringLength(50, ErrorMessage = "Firstname should not exceed 50 characters.")]
        public string? Firstname { get; set; }

        [StringLength(50, ErrorMessage = "Lastname should not exceed 50 characters.")]
        public string? Lastname { get; set; }

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 characters.")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Phone number must be numeric.")]
        public string? Phonenumber { get; set; }

        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid user role.")]
        public UserRole Role { get; set; }

        public CitizenDTO? Citizen { get; set; }
        public OfficerDTO? Officer { get; set; }
        public AdminDTO? Admin { get; set; }
    }

}
