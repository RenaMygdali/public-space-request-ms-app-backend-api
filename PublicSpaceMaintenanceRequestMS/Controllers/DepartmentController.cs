using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public DepartmentController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<DepartmentReadOnlyDTO>> AddDepartmentAsync(DepartmentCreateDTO? departmentCreateDTO)
        {
            if (departmentCreateDTO is null)
            {
                throw new DepartmentCreationException("Department creation request cannot be null.");
            }

            Department? department = await _applicationService.DepartmentService.AddDepartmentAsync(departmentCreateDTO!);

            if (department == null)
            {
                throw new DepartmentCreationException("Failed to create the department.");
            }

            var returnedDepartmentReadOnlyDTO = _mapper.Map<DepartmentReadOnlyDTO>(department);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = returnedDepartmentReadOnlyDTO.Id }, returnedDepartmentReadOnlyDTO);
        } 


        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentReadOnlyDTO>> GetDepartmentById(int id)
        {
            var department = await _applicationService.DepartmentService.GetDepartmentByIdAsync(id);

            if (department is null)
            {
                throw new DepartmentNotFoundException($"Department with ID {id} not found");
            }

            var returnedDepartment = _mapper.Map<DepartmentReadOnlyDTO>(department);
            return Ok(returnedDepartment);
        }


        [HttpGet]
        public async Task<ActionResult> GetAllDepartmentsFilteredAsync(
            [FromQuery] DepartmentFiltersDTO filtersDTO, int pageNumber, int pageSize)
        {
            var filteredDepartments = await _applicationService.DepartmentService
                .GetAllDepartmentsFilteredAsync(pageNumber, pageSize, filtersDTO);

            var departments = await _applicationService.DepartmentService.GetAllDepartmentsAsync();

            if (filteredDepartments == null || filteredDepartments.Count == 0 || departments == null || departments.Count == 0)
            {
                throw new DepartmentNotFoundException("No departments found");
            }

            var totalDepartmentsCount = departments.Count;

            var filteredDepartmentDTOs = _mapper.Map<List<DepartmentDTO>>(filteredDepartments);
            return Ok(new { Departments = filteredDepartmentDTOs, TotalCount = totalDepartmentsCount });
        }


        [HttpGet]
        public async Task<ActionResult> GetAllDepartments()
        {
            var totalDepartments = await _applicationService.DepartmentService.GetAllDepartmentsAsync();

            if (totalDepartments == null || totalDepartments.Count == 0)
            {
                throw new DepartmentNotFoundException("No departments found");
            }

            return Ok(totalDepartments);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentAsync(int id)
        {
            try
            {
                await _applicationService.DepartmentService.DeleteDepartmentAsync(id);
                return Ok(new { message = "Department deleted successfully" });
            }
            catch (DepartmentNotFoundException)
            {
                return NotFound(new { message = "Department not found" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartmentTitleAsync(int id,
             [FromBody] string newTitle)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Καταγραφή σφαλμάτων ή επεξεργασία
                }
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(newTitle))
            {
                throw new InvalidUpdateException("Invalid department update, title is missing");
            }

            try
            {
                await _applicationService.DepartmentService.UpdateDepartmentTitleAsync(id, newTitle);
                return NoContent();     // 204
            }
            catch (DepartmentNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // 500 Internal Server Error
            }
        }
    }
}
