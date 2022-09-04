
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages
{
    public class UnauthorizedToAccessResourceReason : IReasonMessage
    {   
        private readonly object _resource;
        public UnauthorizedToAccessResourceReason(object resource) {
            _resource = resource;
        }
        public string GetMessage()
        {
            return $"requires authorization";
        }
    }
}