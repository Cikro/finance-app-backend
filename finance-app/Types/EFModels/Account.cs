using System;
using System.ComponentModel.DataAnnotations;

namespace finance_app.Types.EFModels
{
    public class Account : DatabaseObject
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Balance { get; set; }
        [Required]
        public uint Type { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public Account Parent_Account { get; set; }
    }
}