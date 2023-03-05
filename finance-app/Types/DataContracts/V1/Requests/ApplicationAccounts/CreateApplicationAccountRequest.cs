using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.Repositories.ApplicationAccounts;
using System.Collections.Generic;

namespace finance_app.Types.DataContracts.V1.Requests.ApplicationAccounts {
    public class CreateApplicationAccountRequest {
        public List<uint> Roles { get; set; }
        public uint AuthorizationUserId { get; set; }
        public uint ApplicationAccountId { get; set; }


    }
}
