namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class DepartmentNotFoundException : Exception
    {
        public DepartmentNotFoundException(string? message) : base(message)
        {
        }

        public DepartmentNotFoundException(int departmentId) : base($"Department with ID {departmentId} not found.")
        {
        }
    }
}
