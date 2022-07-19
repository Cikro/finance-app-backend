using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace finance_app.Types.Repositories.Account
{
    [Table("accounts")]
    public class Account : DatabaseObject, IUserIdResource
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Column("user_id")]
        public uint? UserId { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        [Required]
        public AccountTypeEnum Type { get; set; }
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
            Balance += GetAmountMultiplier(transaction.Type) * transaction.Amount;
            dbContext.Entry(this).Property(x => x.Balance).IsModified = true;
        }

        private int GetAmountMultiplier(Transaction.TransactionTypeEnum t) 
        {
            if (Type == AccountTypeEnum.Asset) 
            {
                switch (t) {
                    case Transaction.TransactionTypeEnum.Debit:
                        return 1;
                    case Transaction.TransactionTypeEnum.Credit:
                        return -1;
                }
            }
            else if (Type == AccountTypeEnum.Liability) 
            {
                switch (t) {
                    case Transaction.TransactionTypeEnum.Debit:
                        return -1;
                    case Transaction.TransactionTypeEnum.Credit:
                        return 1;
                }
            }
            return 0;
        }
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