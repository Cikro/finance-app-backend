using System;
using System.ComponentModel.DataAnnotations;

namespace finance_app.Types.EFModels
{
    public class Account : DatabaseObject
    {
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public double balance { get; set; }
        [Required]
        public uint type { get; set; }
        [Required]
        public string currencyCode { get; set; }
        public Account parent_account { get; set; }
    }
}