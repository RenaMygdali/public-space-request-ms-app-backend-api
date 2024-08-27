using PublicSpaceMaintenanceRequestMS.Data;

namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class InvalidAssignException : Exception
    {
        public InvalidAssignException(string? message) : base($"Invalid request data to assign.")
        {
        }
    }
}
