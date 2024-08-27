using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public UserRole Role { get; set; }

        public virtual Citizen? Citizen { get; set; }
        public virtual Officer? Officer { get; set; }
        public virtual Admin? Admin { get; set; }

        public override string ToString()
        {
            return $"{Username}, {Firstname}, {Lastname}, {Email}, {Phonenumber}, {Role}";

        }
    }
}
