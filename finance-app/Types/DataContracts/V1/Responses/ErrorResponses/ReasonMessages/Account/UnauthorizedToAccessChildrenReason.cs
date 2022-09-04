
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages.Accounts
{
    public class UnauthorizedToAccessChildrenReason : IReasonMessage
    {   
        public string GetMessage()
        {
            return $"has children that you are not authorized to access";
        }
    }
}