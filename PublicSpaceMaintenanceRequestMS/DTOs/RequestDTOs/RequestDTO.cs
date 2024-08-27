using PublicSpaceMaintenanceRequestMS.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs
{
    public class RequestDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "The title cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "The description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(RequestStatus), ErrorMessage = "Invalid request status.")]
        public RequestStatus Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; }

        [Required]
        public int CitizenId { get; set; }

        public string? CitizenUsername { get; set; }

        public int AssignedDepartmentId { get; set; }

        public string? AssignedDepartmentTitle { get; set; }  
    }
}
