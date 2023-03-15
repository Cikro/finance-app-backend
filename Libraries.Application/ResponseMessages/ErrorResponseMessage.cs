using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;

namespace finance_app.Types.Services.V1.ResponseMessages {

    /// <summary>
    /// A Response Message for when an Error Occurs.
    /// In the form of "Error {ActionMessage}. {resourceMessage} {ReasonMessage}"
    /// </summary>
    public class ErrorResponseMessage : IResponseMessage {
        
        /// <summary>
        /// The message to return.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Constructor for the Error Message
        /// </summary>
        /// <param name="action">The Action Message</param>
        /// <param name="resource">The Resource Message</param>
        /// <param name="reason">The Reason Message</param>
        public ErrorResponseMessage(IActionMessage action,
                                    IResourceMessage resource,
                                    IReasonMessage reason) {
            _message = $"Error {action.GetMessage()}. {resource.GetMessage()} {reason.GetMessage()}.";
        }

        public ErrorResponseMessage(IActionMessage action,
                                    IReasonMessage reason) {
            _message = $"Error {action.GetMessage()}. {reason.GetMessage()}.";
        }


        /// <inheritdoc cref="IResponseMessage.GetMessage"/>
        public string GetMessage() {
            return _message;

        }
    }
}