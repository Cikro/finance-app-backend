using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.Validators.Accounts
{
    public class TransactionForJournalEntryRequestsValidator : AbstractValidator<TransactionForJournalEntryRequests>
    {
        public TransactionForJournalEntryRequestsValidator()
        {
            RuleFor(transaction => transaction.AccountId).IsValidDatabaseId();
            RuleFor(transaction => transaction.Amount).GreaterThan(0);
            RuleFor(transaction => transaction.TransactionDate).NotNull();

            RuleFor(transaction => transaction.Type)
                .SetValidator(new EnumDtoValidator<TransactionTypeDtoEnum>());

            RuleFor(transaction => transaction.Notes)
                .MaximumLength(255)
                .ContainsOnlyValidCharacters()
                .When(transaction => transaction.Notes != null);

        }
    }
}
