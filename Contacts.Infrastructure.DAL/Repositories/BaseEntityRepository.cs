using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts.Domain.Common;
using Contacts.Infrastructure.DAL.Core;
using Contacts.Infrastructure.DAL.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.DAL.Repositories
{
    public class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : BaseEntity, new()
    {
        private readonly DbSet<T> _dbSet;
        protected readonly IDatabaseContext DbContext;
        protected IQueryable<T> Queryable => _dbSet.AsQueryable();
        protected IQueryable<T> ReadOnlyQueryable => Queryable.AsNoTracking();

        public IUnitOfWork UnitOfWork => DbContext;

        public BaseEntityRepository(IDatabaseContext databaseContext)
        {
            DbContext = databaseContext;
            _dbSet = DbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public async Task<ICollection<T>> AddRangeAsync(ICollection<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task<T> GetAsync(int entityId)
        {
            return await _dbSet.FindAsync(entityId);
        }

        public virtual async Task<ICollection<T>> GetAllReadOnlyAsync() => await ReadOnlyQueryable.ToListAsync();

        public virtual async Task<ICollection<T>> GetAllAsync() => await Queryable.ToListAsync();

        public void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateFrom(T entity, object from)
        {
            var entry = DbContext.Entry(entity);

            entry.CurrentValues.SetValues(from);
        }

        public async Task<T> UpdateByIdFrom(int id, object from)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found", nameof(id));
            }
            UpdateFrom(entity, from);
            return entity;
        }

        public async Task RemoveById(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found", nameof(id));
            }

            _dbSet.RemoveRange(entity);
        }

        public async Task RemoveRangeByIds(int[] ids)
        {
            var entities = await Queryable.Where(entity => ids.Contains(entity.Id)).ToListAsync();

            if (!entities.Any())
            {
                throw new ArgumentException("No entities with given ids found");
            }

            RemoveRange(entities);
        }

        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
    }

}
