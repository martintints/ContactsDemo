using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Contacts.BL.DTOs.Contact;
using Contacts.Infrastructure.DAL.Core.Repositories;
using FluentValidation.Validators;
// ReSharper disable RedundantJumpStatement

namespace Contacts.BL.Validators.Contact
{
    public class CreateUpdateContactDtoValidator : BaseValidator<CreateUpdateContactDto>, ICreateUpdateContactDtoValidator
    {
        private readonly IContactRepository _contactRepository;

        public CreateUpdateContactDtoValidator(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
            RuleFor(dto => dto.FirstName).MaximumLength(100).NotEmpty();
            RuleFor(dto => dto.LastName).MaximumLength(100).NotEmpty();
            RuleFor(dto => dto.Email).MaximumLength(254).EmailAddress().NotEmpty();
            RuleFor(dto => dto.Sequence).ScalePrecision(precision: 32, scale: 16).NotNull();
            RuleFor(dto => dto.Phone).MaximumLength(254);
            RuleFor(dto => dto).CustomAsync(ValidateDto);
        }

        private async Task ValidateDto(CreateUpdateContactDto dto, CustomContext context,
            CancellationToken cancellationToken)
        {
            if (dto.IsUpdate)
            {
                if (dto.ContactId == null)
                {
                    context.AddFailure(nameof(dto.ContactId), "No contact id specified for update");
                    return;
                }

                var contact = await _contactRepository.GetAsync(dto.ContactId.GetValueOrDefault());

                if (contact == null)
                {
                    context.AddFailure(nameof(dto.ContactId), "Contact not found");
                    return;
                }
            }
        }
    }
}
