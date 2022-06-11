using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories.Account
{
    [Table("accounts")]
    public class Account : DatabaseObject, IUserIdResource
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Column("user_id")]
        public uint UserId { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        [Required]
        public AccountTypeEnum Type { get; set; }
        [Required] 
        [Column("currency_code")]
        public string CurrencyCode { get; set; }

        [Column("Parent_Account_Id")]
        public uint? ParentAccountId { get; set; }
        public bool? Closed { get; set; }
    }

    
    public enum AccountTypeEnum : byte
    {
        Unknown = 0,
        Asset = 1,
        Liability = 2,
        Expense = 3,
        Revenue = 4
    }
}