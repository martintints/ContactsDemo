using Contacts.Infrastructure.DAL.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Contacts.Infrastructure.DAL
{
    /// <summary>
    /// Could be used to mock away database for repositories, ie. make repositories unit testable.
    /// Shouldn't be injected to classes outside of DAL layer (hard dependency on EF).
    /// </summary>
    public interface IDatabaseContext : IUnitOfWork
    {
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
