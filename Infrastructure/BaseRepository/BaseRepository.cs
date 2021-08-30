using Core.Model;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        private readonly DataContext _context;
        private readonly DbSet<TEntity> entities;

        public BaseRepository(DataContext context)
        {
            _context = context;
            entities = context.Set<TEntity>();
        }

        public Task<TEntity> Get(long id, string navigation = null)
        {
            var query = entities.Where(a => a.Id == id);
            if (!string.IsNullOrEmpty(navigation))
            {
                query = query.Include(navigation);
            }

            return query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetByStringProperty(string propertyName, string value) 
        {
            var query = await entities.ToListAsync();
            return query.Where(a => (string)a.GetType().GetProperty(propertyName).GetValue(a) == value)
                        .FirstOrDefault();
        }


        public IQueryable<TEntity> ListQueryable(string navigation = null)
        {
            var query = entities.Select(a => a);
            if (!string.IsNullOrEmpty(navigation))
            {
                query = query.Include(navigation);
            };
            return query;
        }

        public async Task<IList<TEntity>> List(string navigation = null)
        {
            var query = entities.Select(a => a);
            if (!string.IsNullOrEmpty(navigation))
            {
                query = query.Include(navigation);
            };
            return await query.ToListAsync();
        }

        public async Task<TEntity> Save(TEntity entity)
        {
            await entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }
            entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            TEntity entity = await Get(id);
            entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
