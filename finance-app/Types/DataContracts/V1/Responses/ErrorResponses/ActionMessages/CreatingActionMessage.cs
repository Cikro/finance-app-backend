namespace finance_app.Types.DataContracts.V1.Responses.ErrorResponses 
{

    public class CreatingActionMessage : ActionMessage
    {
        public CreatingActionMessage(object resource): base(resource, "creating") { }
    }

}