using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;

namespace finance_app.Types.Services.V1.ResponseMessages.ReasonMessages {
    public class NullReasonMessage : IReasonMessage{

        /// <summary>
        /// The Message to display.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Constructor for the NullReasonMessage.
        /// Creates a message similar to "{ReasourceName} is null"
        /// </summary>
        /// <param name="resourceName">The resource that is null</param>
        public NullReasonMessage(string resourceName) {
            _message = $"{resourceName} is null";

        }
        /// <inheritdoc cref="IResourceMessage.GetMessage"/>
        public string GetMessage() {
            return _message;
        }
    }
}