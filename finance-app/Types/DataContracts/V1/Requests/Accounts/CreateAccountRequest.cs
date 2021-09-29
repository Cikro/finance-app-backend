using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class CreateAccountRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumDto<AccountTypeDtoEnum> Type { get; set; }
        public string CurrencyCode { get; set; }
        public uint? ParentAccountId { get; set; }

    }
}
