namespace finance_app.Types.Repositories.Account.BalanceModifierStrategies
{
    public interface IBalanceModifierStrategy 
    {
        /// <summary>
        /// Gets a new balance from a transaction
        /// </summary>
        /// <returns>The amount to add to the balance</returns>
        public decimal GetModifiedBalance(decimal balance, Transaction.Transaction transaction);
    }
}