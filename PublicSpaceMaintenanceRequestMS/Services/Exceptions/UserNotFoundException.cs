namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string? message) : base(message)
        {
        }

        public UserNotFoundException(int userId) : base($"User with ID {userId} not found.")
        {
        }
    }
}
