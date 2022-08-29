namespace finance_app.Types.DataContracts.V1.Responses.ResourceMessages
{
    public class ResourceWithPropertyMessage : IResourceMessage
    {
        private readonly dynamic _resource;
        private readonly object _property;
        private readonly string _propertyDisplay;
        

        public ResourceWithPropertyMessage(object resource, object property)
        {
            _resource = resource;
            _property = property;
            _propertyDisplay = property?.GetType().Name;
        }

        public string GetMessage()
        {
            return $"{_resource.GetType().Name} with {_propertyDisplay} '{_property}'";
        }
    }
}