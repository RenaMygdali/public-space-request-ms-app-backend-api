namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class Pending
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public virtual Request? Request { get; set; }
    }
}
