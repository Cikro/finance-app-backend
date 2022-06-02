using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Transactions
{
    public class UpdateTransactionRequest
    {   
        public string Notes { get; set; }
    }
}
