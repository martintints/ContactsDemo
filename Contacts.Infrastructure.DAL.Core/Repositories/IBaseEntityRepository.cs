using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Domain.Common;

namespace Contacts.Infrastructure.DAL.Core.Repositories
{
    public interface IBaseEntityRepository<T> where T : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T> AddAsync(T entity);
        Task<T> GetAsync(int entityId);
        Task<ICollection<T>> GetAllReadOnlyAsync();
        Task<ICollection<T>> GetAllAsync();
        void Update(T entity);
        Task RemoveById(int id);
        void UpdateFrom(T entity, object from);
        Task<T> UpdateByIdFrom(int id, object from);
        Task<ICollection<T>> AddRangeAsync(ICollection<T> entities);
        Task RemoveRangeByIds(int[] ids);
        void RemoveRange(IEnumerable<T> entities);
    }
}
