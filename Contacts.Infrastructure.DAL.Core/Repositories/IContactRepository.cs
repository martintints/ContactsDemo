using Contacts.Domain.Contact;

namespace Contacts.Infrastructure.DAL.Core.Repositories
{
    public interface IContactRepository : IBaseEntityRepository<Contact>
    {
    }
}