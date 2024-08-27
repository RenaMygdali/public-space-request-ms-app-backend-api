namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class CitizenNotFoundException : Exception
    {
        public CitizenNotFoundException(string? message) : base(message)
        {
        }

        public CitizenNotFoundException(int citizenId) : base($"Citizen with ID {citizenId} not found.")
        {
        }
    }
}
