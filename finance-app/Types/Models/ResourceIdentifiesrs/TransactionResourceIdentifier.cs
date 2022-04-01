using Microsoft.AspNetCore.Mvc;


namespace finance_app.Types.Models.ResourceIdentifiers {
    public class TransactiResourceIdentifier : UIntResourceIdentifier {

        [FromRoute(Name ="transactionId")]
        public uint Id { get; set; }

        public TransactiResourceIdentifier() {}
        public TransactiResourceIdentifier(uint id) 
        {
            Id = id;
        }
    }
}