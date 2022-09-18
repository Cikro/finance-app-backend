using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;

namespace finance_app.Types.Services.V1.Services.JournalEntries.ResponseMessages.Reasons {
    public class AccountClosedReason : IReasonMessage {
        public string GetMessage() {
            return $"is Closed";
        }
    }
}