using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class CitizenService : ICitizenService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<CitizenService>? _logger;
        private readonly IMapper? _mapper;

        public CitizenService(IUnitOfWork? unitOfWork, ILogger<CitizenService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Citizen?> GetCitizenByIdAsync(int id)
        {
            Citizen? citizen;
            try
            {
                citizen = await _unitOfWork!.CitizenRepository.GetByIdAsync(id);

                if (citizen is null)
                {
                    _logger!.LogWarning($"Citizen with ID {id} not found.");
                    throw new CitizenNotFoundException(id);
                }

                _logger!.LogInformation($"Citizen with ID {citizen.Id} found and returned");

                return citizen;

            }
            catch (CitizenNotFoundException e)
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

        public async Task<Citizen?> GetCitizenByUserIdAsync(int userId)
        {
            Citizen? citizen;
            try
            {
                citizen = await _unitOfWork!.CitizenRepository.GetByUserIdAsync(userId);

                if (citizen is null)
                {
                    _logger!.LogWarning($"Citizen with user ID {userId} not found.");
                    throw new CitizenNotFoundException("Citizen not found");
                }

                _logger!.LogInformation($"Citizen with ID {citizen.Id} found and returned");

                return citizen;

            }
            catch (CitizenNotFoundException e)
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
    }
}
