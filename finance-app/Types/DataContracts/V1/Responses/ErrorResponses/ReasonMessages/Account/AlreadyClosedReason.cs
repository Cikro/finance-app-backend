
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages.Accounts
{
    public class AlreadyClosedReason : IReasonMessage
    {   
        public string GetMessage()
        {
            return $"is already closed";
        }
    }
}