namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class RequestNotFoundException : Exception
    {
        public RequestNotFoundException(string? message) : base(message)
        {
        }

        public RequestNotFoundException(int requestId) : base($"Request with ID {requestId} not found.")
        {
        }
    }
}
