
namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{
    public abstract class ActionMessage : IActionMessage
    {
        private readonly object _resource;
        private readonly string _verb;
        

        public ActionMessage(object resource, string verb)
        {
            _resource = resource;
            _verb = verb;
        }

        public string GetMessage()
        {
            return $"{_verb} {_resource.GetType().Name}";
        }
    }
}