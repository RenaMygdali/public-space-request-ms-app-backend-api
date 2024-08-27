using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(PublicSpaceMaintenanceRequestDbDBContext dbContext) : base(dbContext)
        {
        }


        public async Task<List<Department>> GetAllDepartmentsFilteredAsync(int pageNumber, int pageSize,
            List<Expression<Func<Department, bool>>> predicates)
        {
            int skip = pageSize * (pageNumber - 1);
            IQueryable<Department> query = _dbContext.Departments.AsQueryable();

            // Combine multiple predicates using && operator
            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }


        /// <summary>
        /// Retrieves a department from the database based on its title.
        /// </summary>
        /// <param name="title">The title of the department to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the department object if found, or null if not found.</returns>
        public async Task<Department?> GetByTitleAsync(string title)
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Title == title);

            return department;
        }
    }
}
