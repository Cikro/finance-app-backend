using finance_app.Types.Requests.Accounts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Validators.RequestValidators.Accounts
{
    public class GetAccountsRequestsValidator : AbstractValidator<GetAccountsRequests>
    {
        public GetAccountsRequestsValidator()
        {
            RuleFor(request => request.UserId).NotNull();
            RuleFor(request => request.PageInfo).SetValidator(new PaginationInfoValidator()).When(request => request.PageInfo != null);
        }
    }
}
