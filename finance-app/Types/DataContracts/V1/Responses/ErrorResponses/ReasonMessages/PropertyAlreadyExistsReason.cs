
namespace finance_app.Types.DataContracts.V1.Responses.ReasonMessages
{
    public class PropertyAlreadyExistsReason : IReasonMessage
    {
        private readonly dynamic _resource;
        private readonly string _property;
        private readonly string _propertyDisplay;
        

        public PropertyAlreadyExistsReason(object resource, string property, string propertyDisplay)
        {
            _resource = resource;
            _property = property;
            _propertyDisplay = propertyDisplay;
        }

        public string GetMessage()
        {
            return $"already exists";
        }
    }
}