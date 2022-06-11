using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.DataContracts.V1.Dtos.Enums
{
    public enum TransactionTypeDtoEnum : byte
    {
        Unknown = 0,
        Debit = 1,
        Credit = 2
    }
}
