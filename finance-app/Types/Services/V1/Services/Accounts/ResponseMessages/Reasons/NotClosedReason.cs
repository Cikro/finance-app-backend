using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;

namespace finance_app.Types.Services.V1.Services.Accounts.ResponseMessages.Reasons {
    public class NotClosedReason : IReasonMessage {
        public string GetMessage() {
            return $"was found, but not closed";
        }
    }
}