using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class AdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminService> _logger;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
    }
}
