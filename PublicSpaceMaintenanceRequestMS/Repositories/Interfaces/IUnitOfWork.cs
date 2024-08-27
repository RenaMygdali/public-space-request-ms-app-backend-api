
namespace PublicSpaceMaintenanceRequestMS.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public AdminRepository AdminRepository { get; }
        public CitizenRepository CitizenRepository { get; }
        public DepartmentRepository DepartmentRepository { get; }
        public OfficerRepository OfficerRepository { get; }
        public RequestRepository RequestRepository { get; }
        public UserRepository UserRepository { get; }

        Task<bool> SaveAsync();
    }
}
