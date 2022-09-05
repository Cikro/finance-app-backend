using finance_app.Types.DataContracts.V1.Responses;

namespace finance_app.Types.Services.V1.ResponseMessages {

    /// <summary>
    /// A response message for A Bad Request
    /// </summary>
    public class BadRequestErrorMessage : IResponseMessage 
    {
        /// <summary>
        /// A message for when a Bad Request occurs
        /// </summary>
        /// <returns>A string for a bad request message</returns>
        public string GetMessage() {
            return $"Your input parameters contain errors.";

        }
    }
}