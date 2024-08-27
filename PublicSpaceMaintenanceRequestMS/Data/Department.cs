namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class Department
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Officer> Officers { get; set; } = new HashSet<Officer>();
        public virtual ICollection<Request> Requests { get; set; }  = new HashSet<Request>();
    }
}
