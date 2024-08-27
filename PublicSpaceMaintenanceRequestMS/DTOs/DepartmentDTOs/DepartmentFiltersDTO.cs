namespace PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs
{
    public class DepartmentFiltersDTO
    {
        public string? Title { get; set; }
        public int? MinOfficers { get; set; }
        public int? MaxOfficers { get; set; }
        public int? MinRequests { get; set; }
        public int? MaxRequests { get; set; }
    }
}
