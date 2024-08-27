using PublicSpaceMaintenanceRequestMS.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs
{
    public class RequestUpdateDTO
    {
        [StringLength(200, ErrorMessage = "The title cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "The description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [EnumDataType(typeof(RequestStatus), ErrorMessage = "Invalid request status.")]
        public RequestStatus Status { get; set; }
    }
}
