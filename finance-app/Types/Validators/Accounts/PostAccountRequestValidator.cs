using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests.Accounts;

namespace finance_app.Types.Validators.Accounts
{
    public class PostAccountRequestValidator : AbstractValidator<PostAccountRequest>
    {
        public PostAccountRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty();
        }
    }
}
