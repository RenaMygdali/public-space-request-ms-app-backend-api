using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;
using System.Security.Claims;

namespace PublicSpaceMaintenanceRequestMS.Controllers
{
    public class RequestController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public RequestController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper) : 
            base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRequestAsync([FromBody] RequestSubmitDTO requestDTO)
        {
            try
            {
                if (requestDTO == null)
                {
                    return BadRequest("Request data is null.");
                }

                // Get the user ID from claims
                string? userIdClaimString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaimString))
                {
                    return Unauthorized(new { Message = "User is not authenticated." });
                }

                // Μετατροπή του string userId σε int
                if (!int.TryParse(userIdClaimString, out int userId))
                {
                    return BadRequest("Invalid user ID.");
                }

                // Έλεγχος εάν το userId υπάρχει στον πίνακα Citizens
                var existingCitizen = await _applicationService.CitizenService.GetCitizenByUserIdAsync(userId);

                if (existingCitizen == null)
                {
                    return BadRequest("Citizen does not exist.");
                }

                var citizenId = existingCitizen.Id;

                await _applicationService.RequestService.AddRequestAsync(requestDTO, citizenId);
                return Ok(new { Message = "Request successfully submitted." });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRequestsFilteredAsync(
            [FromQuery] RequestFiltersDTO filtersDTO, int pageNumber, int pageSize)
        {
            var filteredRequests = await _applicationService.RequestService
                .GetAllRequestsFilteredAsync(pageNumber, pageSize, filtersDTO);

            var requests = await _applicationService.RequestService.GetAllRequestsAsync();

            if (filteredRequests == null || filteredRequests.Count == 0 || requests == null || requests.Count == 0)
            {
                throw new RequestNotFoundException("No requests found");
            }

            var totalRequestsCount = requests.Count;

            var filteredRequestsDTOs = _mapper.Map<List<RequestDTO>>(filteredRequests);
            return Ok(new { Requests = filteredRequestsDTOs, TotalCount = totalRequestsCount });
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRequestsWithDetailsFilteredAsync(
            [FromQuery] RequestFiltersDTO filtersDTO, int pageNumber, int pageSize)
        {
            var filteredRequests = await _applicationService.RequestService
                .GetAllRequestsWithDetailsFilteredAsync(pageNumber, pageSize, filtersDTO);

            var totalRequests = await _applicationService.RequestService.GetAllRequestsAsync();

            if (filteredRequests == null || filteredRequests.Count == 0)
            {
                throw new RequestNotFoundException("No requests found");
            }

            var totalRequestsCount = totalRequests.Count;

            return Ok(new { Requests = filteredRequests, TotalCount = totalRequestsCount });
        }

        [HttpGet]
        [Authorize(Roles = "Citizen")]
        public async Task<ActionResult> GetUserRequestsAsync([FromQuery] RequestFiltersDTO filtersDTO, int pageNumber, int pageSize)
        {
            try
            {
                // Λήψη userId από τα claims
                string? userIdClaimString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaimString))
                {
                    return Unauthorized(new { Message = "User is not authenticated." });
                }

                // Μετατροπή userId σε int
                if (!int.TryParse(userIdClaimString, out int userId))
                {
                    return BadRequest("Invalid user ID.");
                }

                // Λήψη citizen με βάση το userId
                var existingCitizen = await _applicationService.CitizenService.GetCitizenByUserIdAsync(userId);

                if (existingCitizen == null)
                {
                    return BadRequest("Citizen does not exist.");
                }

                var citizenId = existingCitizen.Id;

                // Λήψη συνολικού αριθμού αιτημάτων
                var allRequests = await _applicationService.RequestService.GetAllRequestsAsync();
                var totalRequestsCount = allRequests.Count();

                // Λήψη αιτημάτων της τρέχουσας σελίδας
                var filteredRequests = await _applicationService.RequestService
                    .GetUserRequestsFilteredAsync(citizenId, pageNumber, pageSize, filtersDTO);

                return Ok(new { Requests = filteredRequests, TotalCount = totalRequestsCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> AssignRequestToDepartmentAsync([FromBody] RequestAssignDTO requestAssignDTO)
        {
            if (requestAssignDTO == null || requestAssignDTO.RequestId <= 0 || requestAssignDTO.DepartmentId <= 0)
            {
                throw new InvalidAssignException("Invalid data to assign request to department.");
            }

            var result = await _applicationService.RequestService.AssignRequestToDepartmentAsync(requestAssignDTO.RequestId, requestAssignDTO.DepartmentId);

            if (!result)
            {
                throw new RequestNotFoundException(requestAssignDTO.RequestId);
            }

            return Ok("Request successfully assigned to department.");
        }
    }
}
