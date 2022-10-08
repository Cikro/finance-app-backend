using finance_app.Types.Repositories.Transactions;

namespace finance_app.Types.Repositories.Accounts.BalanceModifierStrategies
{
    public interface IBalanceModifierStrategy 
    {
        /// <summary>
        /// Gets a new balance from a transaction
        /// </summary>
        /// <returns>The amount to add to the balance</returns>
        public decimal GetModifiedBalance(decimal balance, Transaction transaction);
    }
}