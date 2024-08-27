using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Controllers
{
    public class OfficerController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OfficerController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper) :
            base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AssignOfficerToDepartmentAsync([FromBody] OfficerAssignDTO officerAssignDTO)
        {
            if (officerAssignDTO == null || officerAssignDTO.DepartmentId <= 0)
            {
                return BadRequest("Invalid data to assign user officer to department.");
            }

            var officer = await _applicationService.OfficerService.GetOfficerByUserIdAsync(officerAssignDTO.UserId);

            if (officer == null)
            {
                throw new UserNotFoundException(officerAssignDTO.UserId);
            }

            // ανάθεση του Officer στο Department
            var result = await _applicationService.OfficerService.AssignOfficerToDepartmentAsync(officer.Id, officerAssignDTO.DepartmentId);

            if (!result)
            {
                return BadRequest("Failed to assign officer to department.");
            }

            return Ok("Officer successfully assigned to department.");
        }

    }
}
