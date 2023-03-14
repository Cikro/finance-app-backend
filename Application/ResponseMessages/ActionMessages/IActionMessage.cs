namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages
{
    /// <summary>
    /// A message for the Action of a response. ex. "Getting" "Creating" etc. 
    /// </summary>
    public interface IActionMessage
    {
        /// <summary>
        /// Gets a message to represent the action being taken.
        /// </summary>
        /// <returns>A string representing the message</returns>
        public string GetMessage();
    }
}