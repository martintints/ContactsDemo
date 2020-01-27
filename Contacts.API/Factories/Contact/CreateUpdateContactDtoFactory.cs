
using AutoMapper;
using Contacts.API.Models.Contact;
using Contacts.BL.DTOs.Contact;
using Contacts.BL.Factories;

namespace Contacts.API.Factories.Contact
{
    public class CreateUpdateContactDtoFactory : BaseFactory<CreateUpdateContactModel, CreateUpdateContactDto>, ICreateUpdateContactDtoFactory
    {
        public CreateUpdateContactDtoFactory(IMapper mapper) : base(mapper)
        {
        }

        public CreateUpdateContactDto Create(CreateUpdateContactModel model, int contactId)
        {
            var dto = Create(model);
            dto.IsUpdate = true;
            dto.ContactId = contactId;
            return dto;
        }
    }
}
