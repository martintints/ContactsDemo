using Contacts.Domain.Contact;
using Contacts.Infrastructure.DAL.Core.Repositories;

namespace Contacts.Infrastructure.DAL.Repositories
{
    public class ContactRepository : BaseEntityRepository<Contact>, IContactRepository
    {
        public ContactRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
