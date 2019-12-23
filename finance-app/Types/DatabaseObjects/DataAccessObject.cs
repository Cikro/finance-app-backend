using System;

namespace finance_app.DataAccessObjects
{
    public class DataAccessObject
    {
        public uint id { get; set; }

        public DateTime dateCreated { get; set; }
        public DateTime dateLastEdited { get; set; }
    }
}
