using System;

namespace finance_app.Types.EFModels
{
    public class DatabaseObject
    {
        public uint Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateLastEdited { get; set; }
    }
}
