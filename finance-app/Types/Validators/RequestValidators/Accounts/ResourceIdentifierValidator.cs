using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests;

namespace finance_app.Types.Validators.RequestValidators.Accounts
{
    public class ResourceIdentifierValidator : AbstractValidator<uint>
    {
        
        public ResourceIdentifierValidator(string resourceName)
        {
            RuleFor(r => r)
            .NotNull().WithMessage($"{resourceName} must not be empty.")
            .Must(r => r > 0).WithMessage($"{resourceName} must be larger than 0");
        }
    }

    public class UserResourceIdentifierValidator : AbstractValidator<UserResourceIdentifier>
    {       
        public UserResourceIdentifierValidator()
        {
            RuleFor(r => r.Id).SetValidator(new ResourceIdentifierValidator("userId"));
        }
    }
}
