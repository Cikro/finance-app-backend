namespace finance_app.Types.DataContracts.V1.Responses {
    
    /// <summary>
    /// A Message to include as part of a response
    /// </summary>
    public interface IResponseMessage {

        /// <summary>
        /// Gets the message for the response.
        /// </summary>
        /// <returns>A response message</returns>
        public string GetMessage();
    }
}