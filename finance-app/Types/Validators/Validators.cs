
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using FluentValidation;

namespace finance_app.Types.Validators {
    public static class Validators {
        private const string VALID_CHARACTERS_REGEX = @"[^a-zA-Z0-9 ""\(\)\.\'\-\+=_\/\?,\!@#$%\^&\*]";

        public static IRuleBuilderOptions<T, uint> IsValidDatabaseId<T>(this IRuleBuilder<T, uint> ruleBuilder) {
            return ruleBuilder.Must(id => id != 0).WithMessage("Invalid Id for {PropertyName}.");
        }
        public static IRuleBuilderOptions<T, uint?> IsValidDatabaseId<T>(this IRuleBuilder<T, uint?> ruleBuilder) {
            return ruleBuilder.Must(id => id != 0).WithMessage("Invalid Id for {PropertyName}.");
        }

        /// <summary>
        /// Validate that the string contains only valid characters to protect against XSS
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> ContainsOnlyValidCharacters<T>(this IRuleBuilder<T, string> ruleBuilder) {
            var re = new Regex(VALID_CHARACTERS_REGEX);
            return ruleBuilder.Must(str => !re.IsMatch(str)).WithMessage($"{{Property name}} contains invalid characters. Valid characters are: {VALID_CHARACTERS_REGEX}");
        }

        /// <summary>
        /// Validate that Dr == Cr in a list of transactions.
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, IEnumerable<TransactionForJournalEntryRequests>> IsValidGroupOfTransactions<T>(this IRuleBuilder<T, IEnumerable<TransactionForJournalEntryRequests>> ruleBuilder) {
            var re = new Regex(VALID_CHARACTERS_REGEX);
            string reason = "Unknown error validating {{Property name}}";
            return ruleBuilder.Must(transactions => {
                    var groupedTransactions = transactions.GroupBy(
                        t => t.Type.Value,
                        t => t.Amount,
                        (key, group) => new  {
                            Key = key, Amount = group.Sum(x => x)
                        })
                        .ToDictionary(x => x.Key, x=> x.Amount);
                    if(groupedTransactions.ContainsKey(TransactionTypeDtoEnum.Unknown)) {
                        reason = $"Transactions in {{Property name}} cannot contain Transaction Type {TransactionTypeDtoEnum.Unknown}.";
                        return false;
                    }
                    if(groupedTransactions[TransactionTypeDtoEnum.Debit] != groupedTransactions[TransactionTypeDtoEnum.Credit]) {
                        reason = $"Error in {{Property name}}. The sum of all transactions with type {TransactionTypeDtoEnum.Debit} must equal the sum of all transactions with type {TransactionTypeDtoEnum.Credit}.";
                        return false;
                    }

                    return true;
                }).WithMessage(reason);
        }
    }
}