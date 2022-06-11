using FluentValidation;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Requests.Transactions;
using System.Linq;

namespace finance_app.Types.Validators.Accounts
{
    public class UpdateTransactionsRequestValidator : AbstractValidator<UpdateTransactionRequest>
    {
        public UpdateTransactionsRequestValidator()
        {
            RuleFor(request => request.Notes)
            .MaximumLength(255)
            .ContainsOnlyValidCharacters().When(request => request.Notes != null);
        }
    }
}
