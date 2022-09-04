using FluentValidation;
using System.Linq;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.Validators.Accounts
{
    public class CreateJournalEntryRequestValidator : AbstractValidator<CreateJournalEntryRequest>
    {
        public CreateJournalEntryRequestValidator()
        {
            RuleFor(request => request.Transactions)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .IsValidGroupOfTransactions();
            RuleForEach(request => request.Transactions)
                .SetValidator(new TransactionForJournalEntryRequestsValidator())
                .When(r => r.Transactions != null);
        }
    }
}
