using Microsoft.AspNetCore.Mvc;


namespace finance_app.Types.Models.ResourceIdentifiers {
    public class TransactionResourceIdentifier : UIntResourceIdentifier {

        [FromRoute(Name ="transactionId")]
        public uint Id { get; set; }

        public TransactionResourceIdentifier() {}
        public TransactionResourceIdentifier(uint id) 
        {
            Id = id;
        }
    }
}