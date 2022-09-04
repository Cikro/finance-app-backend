using System;

namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{
    public class GettingActionMessage: ActionMessage
    {
        public GettingActionMessage(object resource): base(resource, "getting") { }
        public GettingActionMessage(Type resource): base(resource, "getting") { }
    }
}