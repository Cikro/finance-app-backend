
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages
{
    public class UnauthorizedAccessToAccountsReason : IReasonMessage
    {   
        public string GetMessage()
        {
            return $"has some accounts that you ware not authorized to access";
        }
    }
}