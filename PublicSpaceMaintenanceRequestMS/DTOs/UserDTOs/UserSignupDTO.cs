using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs
{
    public class UserSignupDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, " +
            "one lowercase letter, one digit, and one special character.")]
        public string? Password { get; set; }

        [StringLength(50, ErrorMessage = "Firstname should not exceed 50 characters.")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required.")]
        [StringLength(50, ErrorMessage = "Lastname should not exceed 50 characters.")]
        public string? Lastname { get; set; }

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 characters.")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Phone number must be numeric.")]
        public string? Phonenumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid department ID.")]
        public int DepartmentId { get; set; }

        [Required]                                                                   
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid user role.")] 
        public UserRole? Role { get; set; }

        public override string? ToString()
        {
            return $"{Username} {Firstname} {Lastname} {Email} {Password} {Phonenumber} {DepartmentId} {Role!.Value}";
        }
    }
}
