namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{

    public class ClosingActionMessage : ActionMessage
    {
        public ClosingActionMessage(object resource): base(resource, "closing") { }
    }

}