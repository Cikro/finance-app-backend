
namespace finance_app.Types.Services.V1.ResponseMessages.ReasonMessages 
{
    /// <summary>
    /// A message for the Reason an action succeed / failed etc. in response.
    ///  ex. "Already Exists"
    /// </summary>
    public interface IReasonMessage
    {
        /// <summary>
        /// Gets a message to represent the reason.
        /// </summary>
        /// <returns>A string representing the message</returns>
        public string GetMessage();
    }
}