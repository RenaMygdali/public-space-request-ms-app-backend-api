using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<RequestService>? _logger;
        private readonly IMapper? _mapper;

        public RequestService(IUnitOfWork? unitOfWork, ILogger<RequestService>? logger, 
            IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Request> AddRequestAsync(RequestSubmitDTO requestSubmitDTO, int citizenId)
        {
            Request newRequest;

            try
            {
                // Map DTO to entity
                newRequest = _mapper!.Map<Request>(requestSubmitDTO);

                // Ρύθμιση των πεδίων που δεν παρέχονται από τον χρήστη
                newRequest.CreateDate = DateTime.Now;
                newRequest.UpdateDate = DateTime.Now;
                newRequest.Status = RequestStatus.Pending; // default status
                newRequest.CitizenId = citizenId;
                newRequest.AssignedDepartmentId = null;

                var pending = _mapper.Map<Pending>(requestSubmitDTO);
                newRequest.Pending = pending;

                // Adds the new request to the DB
                await _unitOfWork!.RequestRepository.AddAsync(newRequest);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync())
                {
                    throw new Exception("Failed to save request to the database.");
                }

                _logger!.LogInformation($"Request has been successfully submitted.");

                return newRequest;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            Request? request;
            try
            {
                request = await _unitOfWork!.RequestRepository.GetByIdAsync(id);

                if (request is null)
                {
                    _logger!.LogWarning($"Request with ID {id} not found.");
                    throw new RequestNotFoundException(id);
                }

                _logger!.LogInformation($"Request with ID {request.Id} found and returned");

                return request;

            }
            catch (RequestNotFoundException e)
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

        public async Task<List<RequestDTO>> GetAllRequestsFilteredAsync(int pageNumber, int pageSize,
             RequestFiltersDTO requestFiltersDTO)
        {
            List<Request> filteredRequests = new();
            List<Expression<Func<Request, bool>>> predicates = new();

            try
            {
                // Add individual predicates for filtering conditions
                if (!string.IsNullOrEmpty(requestFiltersDTO.Title))
                {
                    predicates.Add(d => d.Title!.Contains(requestFiltersDTO.Title));
                }

                if (requestFiltersDTO.Status.HasValue)
                {
                    predicates.Add(d => d.Status == requestFiltersDTO.Status.Value);
                }

                filteredRequests = await _unitOfWork!.RequestRepository.GetAllRequestsFilteredAsync(pageNumber, pageSize, predicates);

                _logger!.LogInformation("{Message}", "Filtered requests returned successfully.");

                return _mapper!.Map<List<RequestDTO>>(filteredRequests);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<List<RequestDTO>> GetAllRequestsWithDetailsFilteredAsync(int pageNumber, int pageSize,
            RequestFiltersDTO requestFiltersDTO)
        {
            List<Request> filteredRequests = new();
            List<Expression<Func<Request, bool>>> predicates = new();

            try
            {
                if (!string.IsNullOrEmpty(requestFiltersDTO.Title))
                {
                    predicates.Add(d => d.Title!.Contains(requestFiltersDTO.Title));
                }

                if (requestFiltersDTO.Status.HasValue)
                {
                    predicates.Add(d => d.Status == requestFiltersDTO.Status.Value);
                }

                filteredRequests = await _unitOfWork!.RequestRepository.GetAllRequestsWithDetailsFilteredAsync(pageNumber, pageSize, predicates);

                _logger!.LogInformation("{Message}", "Filtered requests returned successfully.");

                return _mapper!.Map<List<RequestDTO>>(filteredRequests);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<List<RequestDTO>> GetAllRequestsAsync()
        {
            try
            {
                var totalRequests = await _unitOfWork!.RequestRepository.GetAllAsync();

                if (totalRequests == null)
                {
                    _logger!.LogWarning($"No requests found.");
                    throw new RequestNotFoundException("No requests found");
                }

                _logger!.LogInformation($"{totalRequests.Count()} requests found and returned");

                var totalRequestsDTOs = _mapper!.Map<List<RequestDTO>>(totalRequests);

                return totalRequestsDTOs;
            }
            catch (RequestNotFoundException e)
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

        public async Task<bool> AssignRequestToDepartmentAsync(int requestId, int departmentId)
        {
            try
            {
                var result = await _unitOfWork!.RequestRepository.AssignRequestAsync(requestId, departmentId);

                if (!result)
                {
                    _logger!.LogWarning($"Request with ID {requestId} not found.");
                    throw new RequestNotFoundException($"Request with ID {requestId} not found");
                }

                _logger!.LogInformation($"Request with ID {requestId} successfully assigned to Department ID {departmentId}.");
                return true;
            }
            catch (RequestNotFoundException e)
            {
                _logger!.LogError("RequestNotFoundException: {Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("Exception: {Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<RequestDTO>> GetUserRequestsFilteredAsync(int citizenId, int pageNumber, int pageSize, RequestFiltersDTO requestFiltersDTO)
        {
            List<Request> filteredRequests = new();
            List<Expression<Func<Request, bool>>> predicates = new();

            try
            {
                // Add predicates for filtering conditions and citizenId
                if (!string.IsNullOrEmpty(requestFiltersDTO.Title))
                {
                    predicates.Add(d => d.Title!.Contains(requestFiltersDTO.Title));
                }

                if (requestFiltersDTO.Status.HasValue)
                {
                    predicates.Add(d => d.Status == requestFiltersDTO.Status.Value);
                }

                // Filter by citizenId
                predicates.Add(r => r.CitizenId == citizenId);

                filteredRequests = await _unitOfWork!.RequestRepository.GetAllRequestsFilteredAsync(pageNumber, pageSize, predicates);

                _logger!.LogInformation("{Message}", "Filtered requests returned successfully.");

                return _mapper!.Map<List<RequestDTO>>(filteredRequests);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

    }
}
