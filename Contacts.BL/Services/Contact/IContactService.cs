using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contacts.BL.DTOs.Contact;
using Contacts.BL.DTOs.Result;
using Contacts.Common.Services;

namespace Contacts.BL.Services.Contact
{
    public interface IContactService : IBaseService
    {
        Task<ResultDto<ICollection<Domain.Contact.Contact>>> GetAllAsync();
        Task<ResultDto<Domain.Contact.Contact>> CreateAsync(CreateUpdateContactDto dto, CancellationToken cancellationToken);
        Task<ResultDto<Domain.Contact.Contact>> GetAsync(int contactId);
        Task<ResultDto<Domain.Contact.Contact>> UpdateAsync(CreateUpdateContactDto dto, CancellationToken cancellationToken);
        Task<ResultDto<bool>> DeleteAsync(int contactId, CancellationToken cancellationToken);
    }
}