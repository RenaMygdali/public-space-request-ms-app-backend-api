using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Data;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class OfficerService : IOfficerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OfficerService> _logger;
        private readonly IMapper _mapper;

        public OfficerService(IUnitOfWork unitOfWork, ILogger<OfficerService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Officer?> GetOfficerByUserIdAsync(int userId)
        {
            Officer? officer;
            try
            {
                officer = await _unitOfWork!.OfficerRepository.GetByUserIdAsync(userId);

                if (officer is null)
                {
                    _logger!.LogWarning($"Officer with user ID {userId} not found.");
                    throw new OfficerNotFoundException("Officer not found");
                }

                _logger!.LogInformation($"Officer with ID {officer.Id} found and returned");

                return officer;

            }
            catch (OfficerNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        public async Task<bool> AssignOfficerToDepartmentAsync(int officerId, int departmentId)
        {
            try
            {
                var result = await _unitOfWork!.OfficerRepository.AssignOfficerAsync(officerId, departmentId);

                if (!result)
                {
                    _logger!.LogWarning($"Failed to assign Officer with ID {officerId} to Department ID {departmentId}.");
                    return false;
                }

                _logger!.LogInformation($"Officer with ID {officerId} successfully assigned to Department ID {departmentId}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger!.LogError("Exception: {Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

    }
}
