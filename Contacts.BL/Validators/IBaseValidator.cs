using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Contacts.BL.Validators
{
    public interface IBaseValidator<T>
    {
        ValidationResult Validate(T instance);
        Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation);
        ValidationResult Validate(ValidationContext<T> context);
        Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation);
    }
}
