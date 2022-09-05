
using System;

namespace finance_app.Types.Services.V1.ResponseMessages.ActionMessages 
{
    /// <summary>
    /// An abstract class for Action Messages.
    /// </summary>
    public abstract class ActionMessage : IActionMessage
    {
        /// <summary>
        /// The message to display
        /// </summary>
        private readonly string _message;
        

        /// <summary>
        /// Constructor for ActionMessage.
        /// Builds a message to displayed in the form "{verb} {resource Name}".
        /// </summary>
        /// <param name="resource">An object of any type</param>
        /// <param name="verb">The name of an action that</param>
        public ActionMessage(object resource, string verb)
        {
            _message = $"{verb} {resource.GetType().Name}";
        }

        /// <summary>
        /// Constructor for ActionMessage.
        /// Builds a message to displayed in the form "{verb} {resourceName}".
        /// </summary>
        /// <param name="resource">A Type</param>
        /// <param name="verb">The name of an action that</param>
        public ActionMessage(Type resource, string verb)
        {
            _message = $"{verb} {resource.Name}";
        }

        /// <inheritdoc cref="IActionMessage.GetMessage"/>
        public string GetMessage()
        {
            return _message;
        }
    }
}