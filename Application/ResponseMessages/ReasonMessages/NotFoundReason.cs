
namespace finance_app.Types.Services.V1.ResponseMessages.ReasonMessages 
{
    /// <summary>
    /// Something could not be found.
    /// </summary>
    public class NotFoundReason : IReasonMessage
    {   
        /// <inheritdoc cref="IReasonMessage.GetMessage"/>
        public string GetMessage()
        {
            return $"could not be found";
        }
    }
}