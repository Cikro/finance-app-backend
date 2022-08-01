using System;

namespace finance_app.Types.Repositories.Account.BalanceModifierStrategies
{

    public class LiabilityBalanceModifierStrategy : IBalanceModifierStrategy {

        /// <inheritdoc cref="IBalanceModifierStrategy.GetModifiedBalance"/>
        public decimal GetModifiedBalance(decimal balance, Transaction.Transaction t) 
        {
            return t.Type switch {
                Transaction.TransactionTypeEnum.Debit => balance -= t.Amount,
                Transaction.TransactionTypeEnum.Credit => balance += t.Amount,
                _ => throw new ArgumentException($"Cannot Apply Transaction of type {t.Type} to Liability."),
            };
        }
    }
}