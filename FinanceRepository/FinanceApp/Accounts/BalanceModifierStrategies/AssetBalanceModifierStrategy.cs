using System;
using finance_app.Types.Repositories.Transactions;

namespace finance_app.Types.Repositories.Accounts.BalanceModifierStrategies
{
    public class AssetBalanceModifierStrategy: IBalanceModifierStrategy {

        /// <inheritdoc cref="IBalanceModifierStrategy.GetModifiedBalance"/>
        public decimal GetModifiedBalance(decimal balance, Transaction t) 
        {
            return t.Type switch {
                TransactionTypeEnum.Debit => balance += t.Amount,
                TransactionTypeEnum.Credit => balance -= t.Amount,
                _ => throw new ArgumentException($"Cannot Apply Transaction of type {t.Type} to Asset Account."),
            };
        }
    }
}