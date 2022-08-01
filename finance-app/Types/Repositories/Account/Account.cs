using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using finance_app.Types.Repositories.Account.BalanceModifierStrategies;

namespace finance_app.Types.Repositories.Account
{
    [Table("accounts")]
    public class Account : DatabaseObject, IUserIdResource
    {
        /// <summary>
        /// A Strategy to modify the Account balance
        /// </summary>
        private IBalanceModifierStrategy _balanceModifierStrategy;

        [Required]
        public string Name { get; set; }

        [Required]
        [Column("user_id")]
        public uint? UserId { get; set; }

        public string Description { get; set; }

        public decimal Balance { get; set; }

        private AccountTypeEnum _type;
        [Required]
        public AccountTypeEnum Type 
        { 
            get {
                return _type;
            } 
            set {
                _type = value;
                _balanceModifierStrategy = GetBalanceModifierStrategy(value);
            } 
        }


        [Required] 
        [Column("currency_code")]
        public string CurrencyCode { get; set; }

        [Column("Parent_Account")]
        public uint? ParentAccountId { get; set; }

        public bool? Closed { get; set; }

        public void ApplyTransaction(DbContext dbContext, Transaction.Transaction transaction) 
        {
            if (Id != transaction.AccountId ) { 
                throw new ArgumentException ($"Will not apply transaction for AccountId {transaction.AccountId} to Account {Id}", nameof(transaction));
            }
            Balance = _balanceModifierStrategy.GetModifiedBalance(Balance, transaction);
            dbContext.Entry(this).Property(x => x.Balance).IsModified = true;
        }


        private static IBalanceModifierStrategy GetBalanceModifierStrategy(AccountTypeEnum type) => type switch
        {
            AccountTypeEnum.Asset    =>  new AssetBalanceModifierStrategy(),
            AccountTypeEnum.Liability    =>  new LiabilityBalanceModifierStrategy(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Error Setting BalanceModifierStrategy. No Strategy exists for Account Type: {type}"),
        };
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