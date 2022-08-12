namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{
    public class CorrectingActionMessage: ActionMessage
    {
        public CorrectingActionMessage(object resource): base(resource, "correcting") { }
    }
}