using AutoMapper;
using Contacts.BL.DTOs.Contact;
using Contacts.Domain.Contact;

namespace Contacts.BL.Factories
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateUpdateContactDto, Contact>();
        }
    }
}
