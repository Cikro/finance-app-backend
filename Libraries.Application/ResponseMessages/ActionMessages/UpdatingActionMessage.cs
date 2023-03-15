namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages
{
    /// <summary>
    /// A class for the "Updating" Message.
    /// </summary>
    public class UpdatingActionMessage: ActionMessage
    {
        /// <summary>
        /// Constructor for a UpdatingActionMessage
        /// </summary>
        /// <param name="resource">The resource you are updating</param>
        public UpdatingActionMessage(object resource): base(resource, "updating") { }
    }
}