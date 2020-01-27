using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Infrastructure.DAL.Core
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
