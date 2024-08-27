namespace PublicSpaceMaintenanceRequestMS.Data
{
    public class Complete
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public virtual Request? Request { get; set; }
    }
}
