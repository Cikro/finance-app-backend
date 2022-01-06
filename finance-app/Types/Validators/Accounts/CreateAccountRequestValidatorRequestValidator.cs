using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.Validators.Accounts
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty();
            RuleFor(request => request.Type).SetValidator(new EnumDtoValidator<AccountTypeDtoEnum>());
            RuleFor(request => request.ParentAccountId)
                .IsValidDatabaseId()
                .When(request => request.ParentAccountId != null);

        }
    }
}
