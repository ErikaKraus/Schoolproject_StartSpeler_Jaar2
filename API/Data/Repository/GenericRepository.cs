using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using API.Models;
//using static API.Data.Repository.GenericRepository<TEntity>;
using System;

namespace API.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly StartspelerContext _context;
        private readonly ILogger<GenericRepository<TEntity>> _logger;


        public GenericRepository(StartspelerContext context, ILogger<GenericRepository<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

       

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = _context.Set<TEntity>();
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij ophalen van alle {typeof(TEntity).Name} objecten: {ex.Message}");
                throw;
            }

        }


        public async Task<TEntity?> GetByIdAsync(object id)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(id);
                if (entity != null)
                {
                    _logger.LogInformation($"{typeof(TEntity).Name} opgehaald met ID {id}: {entity}");
                }
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij ophalen van {typeof(TEntity).Name} met ID {id}: {ex.Message}");
                throw;
            }
        }
            
        
        
        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{typeof(TEntity).Name} toegevoegd: {entity}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij toevoegen van {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }


        public void Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                _context.SaveChanges();
                _logger.LogInformation($"{typeof(TEntity).Name} bijgewerkt: {entity}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij bijwerken van {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }


        public void Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                _logger.LogInformation($"{typeof(TEntity).Name} verwijderd: {entity}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij verwijderen van {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }        
          
        public IQueryable<TEntity> Search()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

      
        


    }
}
