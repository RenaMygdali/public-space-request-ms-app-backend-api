namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class DepartmentCreationException : Exception
    {
        public DepartmentCreationException() { }

        public DepartmentCreationException(string message) : base(message) { }

        public DepartmentCreationException(string message, Exception inner) : base(message, inner) { }
    }
}
