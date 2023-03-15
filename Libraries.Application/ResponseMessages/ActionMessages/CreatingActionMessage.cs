namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages 
{
    /// <summary>
    /// A class for the "Creating" Message.
    /// </summary>
    public class CreatingActionMessage : ActionMessage
    {
        /// <summary>
        /// Constructor for a CreatingActionMessage
        /// </summary>
        /// <param name="resource">The resource you are creating</param>
        public CreatingActionMessage(object resource): base(resource, "creating") { }
    }

}