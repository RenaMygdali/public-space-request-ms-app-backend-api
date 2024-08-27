namespace PublicSpaceMaintenanceRequestMS.Services.Exceptions
{
    public class UserAlreadyExistsException : Exception
    { 
        public UserAlreadyExistsException(string? usernameOrEmail) 
            : base($"User with username or email {usernameOrEmail} already exists.")
        {
        }
        
    }
}
