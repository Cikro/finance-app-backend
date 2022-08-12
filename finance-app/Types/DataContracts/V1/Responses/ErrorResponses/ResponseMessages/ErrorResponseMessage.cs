using finance_app.Types.DataContracts.V1.Responses.ErrorResponses;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.DataContracts.V1.Responses.ResourceMessages;

namespace finance_app.Types.DataContracts.V1.Responses.ResponseMessage
{
    public class ErrorResponseMessage : IResponseMessage
    {
        private readonly IActionMessage _action;
        private readonly IReasonMessage _reason;
        private readonly IResourceMessage _resource;
        public ErrorResponseMessage(IActionMessage action, 
                                    IReasonMessage reason,
                                    IResourceMessage resource) {
            _action = action;
            _reason = reason;
            _resource = resource;
        }

        public string GetMessage()
        {
            return $"Error {_action.GetMessage()}. {_resource.GetMessage()} {_reason.GetMessage()}.";

        }
    }
}