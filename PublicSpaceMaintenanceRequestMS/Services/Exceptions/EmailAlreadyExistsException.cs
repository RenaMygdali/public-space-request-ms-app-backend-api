namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
