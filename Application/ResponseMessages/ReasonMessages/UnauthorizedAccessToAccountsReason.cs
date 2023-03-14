
namespace finance_app.Types.Services.V1.ResponseMessages.ReasonMessages 
{
    /// <summary>
    /// Something has accounts that you can't access.
    /// </summary>
    public class UnauthorizedAccessToAccountsReason : IReasonMessage
    {   
        /// <inheritdoc cref="IReasonMessage.GetMessage"/>
        public string GetMessage()
        {
            return $"has some accounts that you ware not authorized to access";
        }
    }
}