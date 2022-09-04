
using System;

namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{
    public abstract class ActionMessage : IActionMessage
    {
        private readonly string _message;
        

        public ActionMessage(object resource, string verb)
        {
            _message = $"{verb} {resource.GetType().Name}";
        }
        public ActionMessage(Type resource, string verb)
        {
            _message = $"{verb} {resource.Name}";
        }

        public string GetMessage()
        {
            return $"{_message}";
        }
    }
}