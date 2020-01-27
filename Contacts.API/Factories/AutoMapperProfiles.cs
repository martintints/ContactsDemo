using AutoMapper;
using Contacts.API.Models.Contact;
using Contacts.BL.DTOs.Contact;

namespace Contacts.API.Factories
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Model => DTO

            CreateMap<CreateUpdateContactModel, CreateUpdateContactDto>();
            
            #endregion

            #region Domain => Model

            CreateMap<Domain.Contact.Contact, ContactModel>();

            #endregion
        }
    }
}
