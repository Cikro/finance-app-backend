using finance_app.Types.DataContracts.V1.Responses;

namespace finance_app.Types.Services.V1.ResponseMessages {

    /// <summary>
    /// A Message for when something is successful.
    /// </summary>
    public class SuccessResponseMessage : IResponseMessage {

        /// <inheritdoc cref="IResponseMessage.GetMessage"/>s
        public string GetMessage() {
            return $"Success.";
        }
    }
}