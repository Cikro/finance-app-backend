
using System.Collections.Generic;
using System.Linq;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages.Accounts
{
    public class ChildAccountsNotClosedReason : IReasonMessage
    {   
        private readonly IEnumerable<Account> _openAccounts; 
        public ChildAccountsNotClosedReason(IEnumerable<Account> openAccounts) 
        {
            _openAccounts = openAccounts;
        }
        public string GetMessage()
        {
            return $"has children that are not closed. {_openAccounts.Select(a => $"Id: {a.Id}, Name: {a.Name}").ToList()}";
        }
    }
}