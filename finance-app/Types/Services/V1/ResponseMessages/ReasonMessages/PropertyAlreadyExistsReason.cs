
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages
{
    public class PropertyAlreadyExistsReason : IReasonMessage
    {   
        public string GetMessage()
        {
            return $"already exists";
        }
    }
}