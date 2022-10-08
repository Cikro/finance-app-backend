using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using finance_app.Types.Repositories.Transactions;

namespace finance_app.Types.Repositories.JournalEntries
{
    [Table("journal_entries")]
    public class JournalEntry : DatabaseObject, IUserIdResource
    {
        /// <summary>
        /// The user that create the journal entry.
        /// </summary>
        [Required]
        [Column("user_id")]
        public uint? UserId { get; set; }

        private decimal _amount;
        
        /// <summary>
        /// The amount of all the debits and credits
        /// </summary>
        [Required]
        public decimal Amount 
        {
             get {

                // Transactions Exist, ensure value is the same as the Debits / Credits
                var groupedTransactions = GetGroupedTransactions();

                // No Transactions, return value Value
                if (groupedTransactions?.Count() == 0) 
                {
                    return _amount;
                }

                if (groupedTransactions[TransactionTypeEnum.Debit] != groupedTransactions[TransactionTypeEnum.Credit])
                {
                    //TODO: Do I want to throw here?
                    // throw new Exception("Error getting journal amount: Debits != Credits");
                    return -1;
                }

                return groupedTransactions[TransactionTypeEnum.Debit];

                

            }  set {
                if (value < 0) {
                    throw new ArgumentException($"Error Setting JournalEntry Amount of {value}. Value must be greater than 0.", "value");
                }
                // FIXME: Could this be a problem is a mapper tries setting transactions first?
                if (Transactions?.Count() == 0) {
                    throw new ArgumentException($"Error Setting JournalEntry Amount of {value}. Amount cannot be set if the Journal Entry has transactions.", "value");
                }
                _amount = value;
            }
        }

        /// <summary>
        /// If the journal entry has been corrected or not
        /// </summary>
        [Required]
        public bool Corrected { get; set; }

        /// <summary>
        /// If the journal entry was created automatically
        /// </summary>
        [Required]
        [Column("server_generated")]
        public bool ServerGenerated { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }


        #region HelperMethods
        public IEnumerable<Transaction> ReversedTransactions() {
            return Transactions.Select(t => new Transaction {
                Id = null,
                AccountId = t.AccountId,
                UserId = t.UserId,
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
                Notes = $"Correcting transaction with notes: {t.Notes}",
                Type = ReverseTransactionType(t.Type),
            });
        }

        public static TransactionTypeEnum ReverseTransactionType(TransactionTypeEnum t) => t switch
        {
            TransactionTypeEnum.Debit    =>  TransactionTypeEnum.Credit,
            TransactionTypeEnum.Credit    =>  TransactionTypeEnum.Debit,
            _ => throw new ArgumentOutOfRangeException(nameof(t), $"Error Reversing Journal. Not expected Transaction Type Enum : {t}"),
        };

        private Dictionary<TransactionTypeEnum, decimal> GetGroupedTransactions(){
            return Transactions.GroupBy(
                        t => t.Type,
                        t => t.Amount,
                        (key, group) => new  {
                            Key = key, Amount = group.Sum(x => x)
                        })
                        .ToDictionary(x => x.Key, x=> x.Amount);

        }
        #endregion HelperMethods


    }
}