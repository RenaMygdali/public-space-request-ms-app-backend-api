using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PublicSpaceMaintenanceRequestDbDBContext _dbContext;
        private readonly IMapper _mapper;

        public UnitOfWork(PublicSpaceMaintenanceRequestDbDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public AdminRepository AdminRepository => new(_dbContext);
        public CitizenRepository CitizenRepository => new(_dbContext);
        public DepartmentRepository DepartmentRepository => new(_dbContext);
        public OfficerRepository OfficerRepository => new(_dbContext);
        public RequestRepository RequestRepository => new(_dbContext);
        public UserRepository UserRepository => new(_dbContext, _mapper);

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
