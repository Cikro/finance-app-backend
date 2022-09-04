namespace finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages 
{
    /// <summary>
    /// A message describing the resource
    ///  ex. "Already Exists"
    /// </summary>
    public interface IResourceMessage 
    {
        /// <summary>
        /// Gets a message to describe the resource.
        /// </summary>
        /// <returns>A string representing the message</returns>
        public string GetMessage();
    }
}