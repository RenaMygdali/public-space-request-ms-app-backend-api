namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class InvalidUpdateException : Exception
    {
        public InvalidUpdateException(string? message) : base(message)
        {
        }
    }
}
