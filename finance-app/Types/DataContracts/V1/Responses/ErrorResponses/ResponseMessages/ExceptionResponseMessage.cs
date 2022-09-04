using finance_app.Types.DataContracts.V1.Responses.ErrorResponses;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.DataContracts.V1.Responses.ResourceMessages;

namespace finance_app.Types.DataContracts.V1.Responses.ResponseMessage
{
    public class ExceptionResponseMessage : IResponseMessage
    {
        public ExceptionResponseMessage() {}

        public string GetMessage()
        {
            return $"Exceptional Failure! See stack trace If available.";

        }
    }
}