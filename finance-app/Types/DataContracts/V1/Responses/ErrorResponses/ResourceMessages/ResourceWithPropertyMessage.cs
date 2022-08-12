namespace finance_app.Types.DataContracts.V1.Responses.ResourceMessages
{
    public abstract class ResourceWithPropertyMessage : IResourceMessage
    {
        private readonly dynamic _resource;
        private readonly string _property;
        private readonly string _propertyDisplay;
        

        public ResourceWithPropertyMessage(object resource, string property, string propertyDisplay)
        {
            _resource = resource;
            _property = property;
            _propertyDisplay = propertyDisplay;
        }

        public string GetMessage()
        {
            return $"{nameof(_resource)} with {_propertyDisplay} {_property} already exists";
        }
    }
}