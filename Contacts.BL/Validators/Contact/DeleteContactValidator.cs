using System.Threading;
using System.Threading.Tasks;
using Contacts.BL.DTOs.Result;
using Contacts.Infrastructure.DAL.Core.Repositories;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
// ReSharper disable RedundantJumpStatement

namespace Contacts.BL.Validators.Contact
{
    public class DeleteContactValidator : BaseValidator<int>, IDeleteContactValidator
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactValidator(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
            RuleFor(id => id).CustomAsync(ValidateContactId);
        }

        private async Task ValidateContactId(int contactId, CustomContext context, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetAsync(contactId);

            if (contact == null)
            {
                context.AddFailure(new ValidationFailure(nameof(contactId), $"Contact with id {contactId} not found")
                {
                    ErrorCode = nameof(ErrorCodeDto.GeneralNotFound)
                });
                return;
            }
        }
    }
}
