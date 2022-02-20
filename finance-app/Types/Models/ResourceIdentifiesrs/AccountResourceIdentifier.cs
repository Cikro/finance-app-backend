using Microsoft.AspNetCore.Mvc;


namespace finance_app.Types.Models.ResourceIdentifiers {
    public class AccountResourceIdentifier : UIntResourceIdentifier {

        [FromRoute(Name ="accountId")]
        public uint Id { get; set; }
    }
}