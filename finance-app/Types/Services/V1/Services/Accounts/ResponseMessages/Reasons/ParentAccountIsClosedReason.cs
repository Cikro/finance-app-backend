
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Services.V1.Services.Accounts.ResponseMessages.Reasons {
    public class ParentAccountIsClosedReason : IReasonMessage {
        private readonly Account _parentAccount;

        public ParentAccountIsClosedReason(Account parentAccount) {
            _parentAccount = parentAccount;
        }
        public string GetMessage() {
            return $"its parent is closed. {_parentAccount.Id}, {_parentAccount.Name}";
        }
    }
}