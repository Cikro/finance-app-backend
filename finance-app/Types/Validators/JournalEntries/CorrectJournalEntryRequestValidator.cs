using FluentValidation;
using System.Linq;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;

namespace finance_app.Types.Validators.Accounts
{
    public class CorrectJournalEntryRequestValidator : AbstractValidator<CorrectJournalEntryRequest>
    {
        public CorrectJournalEntryRequestValidator()
        {
            RuleFor(request => request.Transactions).NotNull().NotEmpty();
            RuleForEach(request => request.Transactions).SetValidator(new TransactionForJournalEntryRequestsValidator());
            RuleFor(request => request.Transactions).IsValidGroupOfTransactions();
        }
    }
}
