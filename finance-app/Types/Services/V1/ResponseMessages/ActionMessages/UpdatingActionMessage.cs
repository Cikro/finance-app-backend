namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{
    public class UpdatingActionMessage: ActionMessage
    {
        public UpdatingActionMessage(object resource): base(resource, "updating") { }
    }
}