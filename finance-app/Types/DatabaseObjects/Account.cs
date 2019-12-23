using System;

namespace finance_app.DataAccessObjects
{
    public class Account : DataAccessObject
    {
        public string name { get; set; }
        public string description { get; set; }
        public double balance { get; set; }
        public uint type { get; set; }
        public string currencyCode { get; set; }
        public Account parent_account { get; set; }
    }
}