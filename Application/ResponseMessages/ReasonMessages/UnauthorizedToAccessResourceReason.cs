
namespace finance_app.Types.Services.V1.ResponseMessages.ReasonMessages 
{
    /// <summary>
    /// You are not authorized to access something
    /// </summary>
    public class UnauthorizedToAccessResourceReason : IReasonMessage
    {   
        /// <inheritdoc cref="IReasonMessage.GetMessage"/>
        public string GetMessage()
        {
            return $"requires authorization that you do not have";
        }
    }
}