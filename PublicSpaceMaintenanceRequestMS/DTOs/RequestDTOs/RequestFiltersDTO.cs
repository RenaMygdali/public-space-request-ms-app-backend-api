using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs
{
    public class RequestFiltersDTO
    {
        public string? Title { get; set; }
        public RequestStatus? Status { get; set; }

        // Method to convert RequestStatus to string
        public string? GetStatusAsString()
        {
            return Status?.ToString();
        }
    }
}
