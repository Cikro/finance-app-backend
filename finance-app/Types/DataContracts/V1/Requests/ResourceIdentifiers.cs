using Microsoft.AspNetCore.Mvc;

namespace finance_app.Types.DataContracts.V1.Requests {
    public class UserResourceIdentifier {

        [FromRoute(Name ="userId")]
        public uint Id { get; set; }
    }
}