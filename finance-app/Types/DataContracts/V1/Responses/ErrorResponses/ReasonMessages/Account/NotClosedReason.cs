
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages.Accounts
{
    public class NotClosedReason : IReasonMessage
    {   
        public string GetMessage()
        {
            return $"was found, but not closed";
        }
    }
}