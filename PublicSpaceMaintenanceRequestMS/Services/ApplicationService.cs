using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class ApplicationService : IApplicationService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ILogger<UserService>? _userLogger;
        protected readonly ILogger<DepartmentService>? _departmentLogger;
        protected readonly ILogger<RequestService>? _requestLogger;
        protected readonly ILogger<CitizenService>? _citizenLogger;
        protected readonly ILogger<OfficerService>? _officerLogger;

        public ApplicationService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<UserService>? userLogger, 
            ILogger<DepartmentService>? departmentLogger,
            ILogger<RequestService>? requestLogger,
            ILogger<CitizenService>? citizenLogger,
            ILogger<OfficerService> officerLogger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userLogger = userLogger;
            _departmentLogger = departmentLogger;
            _requestLogger = requestLogger;
            _citizenLogger = citizenLogger;
            _officerLogger = officerLogger;
        }

        public UserService UserService => new(_unitOfWork, _userLogger, _mapper);

        public CitizenService CitizenService => new(_unitOfWork, _citizenLogger, _mapper);

        public OfficerService OfficerService => new(_unitOfWork, _officerLogger, _mapper);

        //public AdminService AdminService => new(_unitOfWork, _logger, _mapper);

        public RequestService RequestService => new(_unitOfWork, _requestLogger, _mapper);

        public DepartmentService DepartmentService => new(_unitOfWork, _departmentLogger, _mapper);
    }
}
