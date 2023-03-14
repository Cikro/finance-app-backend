using Microsoft.AspNetCore.Mvc;


namespace finance_app.Types.Models.ResourceIdentifiers {
    public class JournalEntryResourceIdentifier : UIntResourceIdentifier {

        [FromRoute(Name ="journalEntryId")]
        public uint Id { get; set; }

        public JournalEntryResourceIdentifier() {}
        public JournalEntryResourceIdentifier(uint id) 
        {
            Id = id;
        }
    }
}