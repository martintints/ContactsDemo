using Contacts.API.Models.Contact;
using Contacts.BL.DTOs.Contact;
using Contacts.BL.Factories;

namespace Contacts.API.Factories.Contact
{
    public interface ICreateUpdateContactDtoFactory : IBaseFactory<CreateUpdateContactModel, CreateUpdateContactDto>
    {
        CreateUpdateContactDto Create(CreateUpdateContactModel model, int contactId);
    }
}
