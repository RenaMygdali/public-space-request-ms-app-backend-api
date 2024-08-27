using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class Request
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int CitizenId { get; set; }
        public virtual Citizen? Citizen { get; set; }

        public int? AssignedDepartmentId { get; set; }
        public virtual Department? AssignedDepartment { get; set; }

        public virtual Pending? Pending { get; set; }
        public virtual InProgress? InProgress { get; set; }
        public virtual Complete? Complete { get; set; }
    }
}
