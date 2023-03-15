
namespace finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages
{
    public class ResourceWithPropertyMessage : IResourceMessage
    {

        /// <summary>
        /// The Message to display.
        /// </summary>
        private readonly string _message;
        
        /// <summary>
        /// Constructor for the ResourceWithPropertyMessage.
        /// Creates a message similar to "{resource} with {propertyName} {propertyValue}"
        /// </summary>
        /// <param name="resource">The resource you resource</param>
        /// <param name="propertyDisplay">A string representation of the property. ex. "Name"</param>
        /// <param name="property">The property itself</param>
        public ResourceWithPropertyMessage(object resource, string propertyDisplay, object property)
        {
            _message =  $"{resource.GetType().Name} with {propertyDisplay} '{property}'";
        
        }
        /// <inheritdoc cref="IResourceMessage.GetMessage"/>
        public string GetMessage()
        {
            return _message;
        }
    }
}