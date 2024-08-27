using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly PublicSpaceMaintenanceRequestDbDBContext _dbContext;
        private readonly DbSet<T> _dbSet;

        protected BaseRepository(PublicSpaceMaintenanceRequestDbDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        /// <summary>
        /// Adds a new entity to the database asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous Add operation.</returns>
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Adds a collection of entities to the database asynchronously.
        /// </summary>
        /// <param name="entities">The collection of entities to add.</param>
        /// <returns>A task that represents the asynchronous AddRange operation.</returns>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Updates an entity in the database asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public virtual void UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            //await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity from the database asynchronously based on its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous Delete operation. The task result indicates whether the deletion was successful (true) or the entity was not found (false).</returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves all entities from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all entities.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        /// <summary>
        /// Retrieves an entity from the database asynchronously based on its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified ID, or null if not found.</returns>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        /// <summary>
        /// Retrieves the count of entities in the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities in the database.</returns>
        public virtual async Task<int> GetCountAsync()
        {
            var count = await _dbSet.CountAsync();
            return count;
        }
    }
}
