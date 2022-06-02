using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Requests.Transactions;

namespace finance_app.Types.Validators.Accounts
{
    public class GetTransactionsRequestValidator : AbstractValidator<GetTransactionsRequest>
    {
        public GetTransactionsRequestValidator()
        {
            RuleFor(request => request.PageInfo).SetValidator(new PaginationInfoValidator()).When(request => request.PageInfo != null);
        }
    }
}
