
namespace finance_app.Types.Services.V1.ResponseMessages.ReasonMessages 
{
    /// <summary>
    /// Something Already Exists reason.
    /// </summary>
    public class PropertyAlreadyExistsReason : IReasonMessage
    {   
        /// <inheritdoc cref="IReasonMessage.GetMessage"/>
        public string GetMessage()
        {
            return $"already exists";
        }
    }
}