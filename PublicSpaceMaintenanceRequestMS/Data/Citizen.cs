namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class Citizen
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Request> Requests { get; } = new HashSet<Request>();
    }
}
