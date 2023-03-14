using System;

namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages
{
    /// <summary>
    /// A class for the "Getting" Message.
    /// </summary>
    public class GettingActionMessage: ActionMessage
    {
        /// <summary>
        /// Constructor for a GettingActionMessage
        /// </summary>
        /// <param name="resource">The resource you are getting</param>
        public GettingActionMessage(object resource): base(resource, "getting") { }

        /// <summary>
        /// Constructor for a GettingActionMessage
        /// </summary>
        /// <param name="resource">The type of resource you are getting</param>
        public GettingActionMessage(Type resource): base(resource, "getting") { }
    }
}