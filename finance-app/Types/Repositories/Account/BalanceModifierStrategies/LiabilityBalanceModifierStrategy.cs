using System;

namespace finance_app.Types.Repositories.Account.BalanceModifierStrategies
{

    public class LiabilityBalanceModifierStrategy : IBalanceModifierStrategy {

        /// <inheritdoc cref="IBalanceModifierStrategy.GetBalanceModifier"/>
        public decimal GetModifiedBalance(decimal balance, Transaction.Transaction t) 
        {
            switch (t.Type) {
                case Transaction.TransactionTypeEnum.Debit:
                    return balance -= t.Amount;
                case Transaction.TransactionTypeEnum.Credit:
                    return balance += t.Amount;
                default:
                    throw new ArgumentException($"Cannot Apply Transaction of type {t.Type} to Liability.");
            }
        }
    }
}