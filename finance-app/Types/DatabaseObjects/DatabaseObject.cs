using System;

namespace finance_app.DatabaseObjects
{
    public class DatabaseObject
    {
        public uint id { get; set; }

        public DateTime dateCreated { get; set; }
        public DateTime dateLastEdited { get; set; }
    }
}
