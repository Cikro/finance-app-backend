using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories.Account
{
    [Table("accounts")]
    public class Account : DatabaseObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public uint User_Id { get; set; }
        public string Description { get; set; }
        public Decimal Balance { get; set; }
        [Required]
        public AccountTypeEnum Type { get; set; }
        [Required] 
        public string Currency_Code { get; set; }
        public uint? Parent_Account_Id { get; set; }
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