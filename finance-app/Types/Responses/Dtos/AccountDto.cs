using finance_app.Types.Responses.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Responses.Dtos
{
    public class AccountDto : BaseDto
    {
        public string Name { get; set; }
        public uint UserId { get; set; }
        public string Description { get; set; }
        public Decimal Balance { get; set; }
        public EnumDto<AccountTypeDtoEnum> Type { get; set; }
        
        public string CurrencyCode { get; set; }
        public uint? ParentAccountId { get; set; }
    }
}
