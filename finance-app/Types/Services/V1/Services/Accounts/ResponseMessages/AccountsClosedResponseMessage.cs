using System.Collections.Generic;
using System.Linq;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Services.V1.Services.Accounts.ResponseMessages {
    public class AccountsClosedResponseMessage : IResponseMessage {
        private readonly IEnumerable<Account> _accountsClosed;
        public AccountsClosedResponseMessage(IEnumerable<Account> accountsClosed) {
            _accountsClosed = accountsClosed;
        }

        public string GetMessage() {
            return $"{_accountsClosed.Count()} Accounts Closed";
        }
    }
}