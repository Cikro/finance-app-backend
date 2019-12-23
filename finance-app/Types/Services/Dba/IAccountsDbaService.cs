using System.Collections.Generic;
using finance_app.DataAccessObjects;

namespace finance_app.Services {

    public interface IAccountsDbaService
    {
        Task<List<Account>> GetAccounts();
    }

}
