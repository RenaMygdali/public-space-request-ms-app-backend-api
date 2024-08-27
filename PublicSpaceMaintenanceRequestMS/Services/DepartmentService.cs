using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<DepartmentService>? _logger;
        private readonly IMapper? _mapper;

        public DepartmentService(IUnitOfWork? unitOfWork, ILogger<DepartmentService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        
        public async Task<Department> AddDepartmentAsync(DepartmentCreateDTO departmentCreateDTO)
        {
            Department newDepartment;
            Department? existingDepartment;

            try
            {
                newDepartment = _mapper!.Map<Department>(departmentCreateDTO);

                existingDepartment = await _unitOfWork!.DepartmentRepository.GetByTitleAsync(newDepartment.Title!);

                if (existingDepartment != null)
                {
                    _logger!.LogWarning($"A department with title {newDepartment.Title} already exists.");
                    throw new DepartmentAlreadyExistsException(newDepartment.Title);
                }

                // Adds the new department to the DB
                await _unitOfWork.DepartmentRepository.AddAsync(newDepartment);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync())
                {
                    throw new Exception("Failed to save department to the database.");
                }

                _logger!.LogInformation($"Department with title {newDepartment.Title} has been successfully registered.");

                return newDepartment;
            } catch (DepartmentAlreadyExistsException e)
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



        public async Task<List<DepartmentDTO>> GetAllDepartmentsFilteredAsync(int pageNumber, int pageSize,
             DepartmentFiltersDTO departmentFiltersDTO)
        {
            List<Department> filteredDepartments = new();
            List<Expression<Func<Department, bool>>> predicates = new();

            try
            {
                // Add individual predicates for filtering conditions
                if (!string.IsNullOrEmpty(departmentFiltersDTO.Title))
                {
                    predicates.Add(d => d.Title!.Contains(departmentFiltersDTO.Title));
                }

                if (departmentFiltersDTO.MinOfficers.HasValue)
                {
                    predicates.Add(d => d.Officers.Count >= departmentFiltersDTO.MinOfficers.Value);
                }

                if (departmentFiltersDTO.MaxOfficers.HasValue)
                {
                    predicates.Add(d => d.Officers.Count <= departmentFiltersDTO.MaxOfficers.Value);
                }

                if (departmentFiltersDTO.MinRequests.HasValue)
                {
                    predicates.Add(d => d.Requests.Count >= departmentFiltersDTO.MinRequests.Value);
                }

                if (departmentFiltersDTO.MaxRequests.HasValue)
                {
                    predicates.Add(d => d.Requests.Count <= departmentFiltersDTO.MaxRequests.Value);
                }

                filteredDepartments = await _unitOfWork!.DepartmentRepository.GetAllDepartmentsFilteredAsync(pageNumber, pageSize, predicates);

                _logger!.LogInformation("{Message}", "Filtered departments returned successfully.");

                return _mapper!.Map<List<DepartmentDTO>>(filteredDepartments);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            Department? department;
            try
            {
                department = await _unitOfWork!.DepartmentRepository.GetByIdAsync(id);

                if (department is null)
                {
                    _logger!.LogWarning($"Department with ID {id} not found.");
                    throw new DepartmentNotFoundException(id);
                }

                _logger!.LogInformation($"Department with title {department.Title} found and returned");

                return department;

            }
            catch (DepartmentNotFoundException e)
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


        public async Task<List<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            try
            {
                var totalDepartments = await _unitOfWork!.DepartmentRepository.GetAllAsync();

                if (totalDepartments == null)
                {
                    _logger!.LogWarning($"No departments found.");
                    throw new DepartmentNotFoundException("No departments found");
                }

                _logger!.LogInformation($"{totalDepartments.Count()} departments found and returned");

                var totalDepartmentsDTOs = _mapper!.Map<List<DepartmentDTO>>(totalDepartments);

                return totalDepartmentsDTOs;
            }
            catch (DepartmentNotFoundException e)
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



        public async Task DeleteDepartmentAsync(int id)
        {
            try
            {
                bool result = await _unitOfWork!.DepartmentRepository.DeleteAsync(id);

                if (!result)
                {
                    _logger!.LogWarning($"Department with ID {id} not found.");
                    throw new DepartmentNotFoundException(id);
                }

                _logger!.LogInformation($"Department with id {id} deleted successfully.");

            }
            catch (DepartmentNotFoundException e)
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


        public async Task UpdateDepartmentTitleAsync(int id, string? newTitle) 
        {
            try
            {
                var department = await _unitOfWork!.DepartmentRepository.GetByIdAsync(id);

                if (department == null)
                {
                    _logger!.LogWarning($"Department with ID {id} not found.");
                    throw new DepartmentNotFoundException($"Department with id {id} not found");
                }

                department.Title = newTitle;

                _unitOfWork!.DepartmentRepository.UpdateAsync(department);
                await _unitOfWork!.SaveAsync();
                _logger!.LogInformation($"Department with id {id} is updated successfully.");
            }
            catch (DepartmentNotFoundException e)
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
