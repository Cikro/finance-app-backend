namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages
{
    /// <summary>
    /// A class for the "Updating" Message.
    /// </summary>
    public class LogginInActionMessage: ActionMessage
    {
        /// <summary>
        /// Constructor for a LogginInActionMessage
        /// </summary>
        /// <param name="resource"></param>
        public LogginInActionMessage(object resource): base(resource, "Loggin In") { }
    }
}