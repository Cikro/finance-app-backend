namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages 
{
 
    /// <summary>
    /// A class for the "Correcting" Message.
    /// </summary>
    public class CorrectingActionMessage: ActionMessage
    {
        /// <summary>
        /// Constructor for a CorrectingActionMessage
        /// </summary>
        /// <param name="resource">The resource you are correcting</param>
        public CorrectingActionMessage(object resource): base(resource, "correcting") { }
    }
}