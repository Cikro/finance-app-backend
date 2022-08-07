using FluentValidation;
using System.Linq;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.Validators.Accounts
{
    public class GetRecentJournalEntriesRequestValidator : AbstractValidator<GetRecentJournalEntriesRequest>
    {
        public GetRecentJournalEntriesRequestValidator()
        {
            RuleFor(request => request.PageInfo).SetValidator(new PaginationInfoValidator()).When(request => request.PageInfo != null);
        }
    }
}
