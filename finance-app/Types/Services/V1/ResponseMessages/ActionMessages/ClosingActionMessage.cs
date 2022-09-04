namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages 
{
    /// <summary>
    /// A class for the "Closing" Message.
    /// </summary>
    public class ClosingActionMessage : ActionMessage
    {
        /// <summary>
        /// Constructor for a ClosingActionMessage
        /// </summary>
        /// <param name="resource">The resource you are closing</param>
        public ClosingActionMessage(object resource): base(resource, "closing") { }
    }

}