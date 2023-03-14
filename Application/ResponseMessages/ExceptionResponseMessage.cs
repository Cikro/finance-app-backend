using finance_app.Types.DataContracts.V1.Responses;

namespace finance_app.Types.Services.V1.ResponseMessages 
{

    /// <summary>
    /// A Message for when exceptions occur
    /// </summary>
    public class ExceptionResponseMessage : IResponseMessage 
    {

        /// <inheritdoc cref="IResponseMessage.GetMessage"/>
        public string GetMessage() {
            return $"Exceptional Failure! See stack trace If available.";

        }
    }
}