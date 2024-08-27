namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class DepartmentAlreadyExistsException : Exception
    {
        public DepartmentAlreadyExistsException(string? title) : 
            base($"Department with title {title} already exists.")
        {
        }
    }
}
