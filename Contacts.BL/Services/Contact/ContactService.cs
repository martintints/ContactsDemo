using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contacts.BL.DTOs.Contact;
using Contacts.BL.DTOs.Result;
using Contacts.BL.Factories;
using Contacts.BL.Validators.Contact;
using Contacts.Common.Services;
using Contacts.Infrastructure.DAL.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Contacts.BL.Services.Contact
{

    public class ContactService : BaseService, IContactService
    {
        private readonly ILogger<ContactService> _logger;

        private readonly IContactRepository _contactRepository;

        private readonly ICreateUpdateContactDtoValidator _createUpdateContactDtoValidator;
        private readonly IBaseFactory<CreateUpdateContactDto, Domain.Contact.Contact> _contactFactory;

        private readonly IDeleteContactValidator _deleteContactValidator;

        public ContactService(
            IContactRepository contactRepository,
            ICreateUpdateContactDtoValidator createUpdateContactDtoValidator, 
            ILogger<ContactService> logger, 
            IBaseFactory<CreateUpdateContactDto, Domain.Contact.Contact> contactFactory, 
            IDeleteContactValidator deleteContactValidator)
        {
            _contactRepository = contactRepository;
            _createUpdateContactDtoValidator = createUpdateContactDtoValidator;
            _logger = logger;
            _contactFactory = contactFactory;
            _deleteContactValidator = deleteContactValidator;
        }

        public async Task<ResultDto<ICollection<Domain.Contact.Contact>>> GetAllAsync()
        {
            //SQLLite does not support this sort on query (decimal not native)
            //System.NotSupportedException: SQLite cannot order by expressions of type 'decimal'. 
            var contacts = (await _contactRepository.GetAllAsync()).OrderBy(c => c.Sequence).ToList();
            var result = new ResultDto<ICollection<Domain.Contact.Contact>>();

            if (!contacts.Any())
            {
                result.AddError(ErrorCodeDto.GeneralNotFound);
                return result;
            }

            result.AddData(contacts);
            return result;
        }


        public async Task<ResultDto<Domain.Contact.Contact>> GetAsync(int contactId)
        {
            var contact = await _contactRepository.GetAsync(contactId);
            var result = new ResultDto<Domain.Contact.Contact>(contact);

            if (result.Data == null)
            {
                result.AddError(ErrorCodeDto.GeneralNotFound);
                return result;
            }

            return result;
        }

        public async Task<ResultDto<Domain.Contact.Contact>> CreateAsync(CreateUpdateContactDto dto, CancellationToken cancellationToken)
        {
            var validationResult = _createUpdateContactDtoValidator.Validate(dto);
            var result = new ResultDto<Domain.Contact.Contact>(validationResult);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Contact creation validation failed: {message}", validationResult.ToString());
                return result;
            }

            var contact = _contactFactory.Create(dto);

            await _contactRepository.AddAsync(contact);
            await _contactRepository.UnitOfWork.SaveChangesAsync(true, cancellationToken);

            result.AddData(contact);
            return result;
        }

        public async Task<ResultDto<Domain.Contact.Contact>> UpdateAsync(CreateUpdateContactDto dto, CancellationToken cancellationToken)
        {
            var validationResult = await _createUpdateContactDtoValidator.ValidateAsync(dto, cancellationToken);
            var result = new ResultDto<Domain.Contact.Contact>(validationResult);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Contact update validation failed: {message}", validationResult.ToString());
                return result;
            }

            var contact = await _contactRepository.UpdateByIdFrom(dto.ContactId.GetValueOrDefault(), dto);

            await _contactRepository.UnitOfWork.SaveChangesAsync(true, cancellationToken);

            result.AddData(contact);
            return result;
        }

        public async Task<ResultDto<bool>> DeleteAsync(int contactId, CancellationToken cancellationToken)
        {
            var validationResult = await _deleteContactValidator.ValidateAsync(contactId, cancellationToken);
            var result = new ResultDto<bool>(validationResult);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Contact deletion failed: {message}", validationResult.ToString());
                return result;
            }

            await _contactRepository.RemoveById(contactId);
            await _contactRepository.UnitOfWork.SaveChangesAsync();

            result.AddData(true);
            return result;
        }
    }
}
