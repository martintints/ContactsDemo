using FluentValidation;

namespace Contacts.BL.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>, IBaseValidator<T>
    {
        public BaseValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
        }
    }
}
