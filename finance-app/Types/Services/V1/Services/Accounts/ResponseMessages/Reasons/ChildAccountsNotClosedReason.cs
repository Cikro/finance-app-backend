
using System.Collections.Generic;
using System.Linq;
using finance_app.Types.Repositories.Accounts;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;

namespace finance_app.Types.Services.V1.Services.Accounts.ResponseMessages.Reasons {
    public class ChildAccountsNotClosedReason : IReasonMessage {
        private readonly IEnumerable<Account> _openAccounts;
        public ChildAccountsNotClosedReason(IEnumerable<Account> openAccounts) {
            _openAccounts = openAccounts;
        }
        public string GetMessage() {
            return $"has children that are not closed. {_openAccounts.Select(a => $"Id: {a.Id}, Name: {a.Name}").ToList()}";
        }
    }
}