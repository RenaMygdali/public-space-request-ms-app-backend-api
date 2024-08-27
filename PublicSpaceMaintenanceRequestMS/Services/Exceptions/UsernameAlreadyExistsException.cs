namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
