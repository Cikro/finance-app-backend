using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests.Accounts;

namespace finance_app.Types.Validators.RequestValidators.Accounts
{
    public class GetAccountsRequestsValidator : AbstractValidator<GetAccountsRequests>
    {
        public GetAccountsRequestsValidator()
        {
            RuleFor(request => request.UserId).NotNull();
            _ = RuleFor(request => request.PageInfo).SetValidator(new PaginationInfoValidator()).When(request => request.PageInfo != null);
        }
    }
}
