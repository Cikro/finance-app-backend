using System;
using System.Security;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.DataContracts.V1.Dtos
{
    public class AccountDto : BaseDto
    {

        /// <summary>
        /// The name of the Account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user that owns the Account
        /// </summary>
        public uint UserId { get; set; }

        /// <summary>
        /// The Account's Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Account's Balance
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// The type of the account
        /// </summary>
        public EnumDto<AccountTypeDtoEnum> Type { get; set; }

        /// <summary>
        /// The Currency the Account is storing
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Is the Account Closed
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// The Id of the Account's Parent
        /// </summary>
        /// <value></value>
        public uint? ParentAccountId { get; set; }
    }
}
