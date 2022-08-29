
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages
{
    public class NotFoundReason : IReasonMessage
    {   
        public string GetMessage()
        {
            return $"could not be found";
        }
    }
}