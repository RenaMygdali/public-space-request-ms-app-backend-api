namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class OfficerNotFoundException : Exception
    {
        public OfficerNotFoundException(string? message) : base(message)
        {
        }

        public OfficerNotFoundException(int officerId) : base($"Officer with ID {officerId} not found.")
        {
        }
    }
}
