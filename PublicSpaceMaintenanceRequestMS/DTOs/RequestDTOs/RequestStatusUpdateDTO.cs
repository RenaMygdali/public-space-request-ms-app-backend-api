using PublicSpaceMaintenanceRequestMS.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs
{
    public class RequestStatusUpdateDTO
    {
        [Required]
        public int RequestId { get; set; }      // να αποφασίσω αν χρειάζεται ή όχι

        [Required(ErrorMessage = "Status is required.")]
        public RequestStatus Status { get; set; }
    }
}
