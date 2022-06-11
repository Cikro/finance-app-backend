namespace finance_app.Types.DataContracts.V1.Requests.Transactions
{
    /// <summary>
    /// A request to update a transaction
    /// </summary>
    public class UpdateTransactionRequest
    {   

        /// <summary>
        /// Notes about a transaction
        /// </summary>
        public string Notes { get; set; }
    }
}
