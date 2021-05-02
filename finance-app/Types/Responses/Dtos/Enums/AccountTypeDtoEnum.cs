using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Responses.Dtos.Enums
{
    public enum AccountTypeDtoEnum : byte
    {
        Unknown = 0,
        Asset = 1,
        Liability = 2,
        Expense = 3,
        Revenue = 4
    }
}
