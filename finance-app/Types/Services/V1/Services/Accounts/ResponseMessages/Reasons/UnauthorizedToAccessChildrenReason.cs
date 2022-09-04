using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;

namespace finance_app.Types.Services.V1.Services.Accounts.ResponseMessages.Reasons {
    public class UnauthorizedToAccessChildrenReason : IReasonMessage {
        public string GetMessage() {
            return $"has children that you are not authorized to access";
        }
    }
}