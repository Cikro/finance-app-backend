using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.EFModels
{
    [Table("Accounts")]
    public class Account : DatabaseObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public uint UserId { get; set; }
        public string Description { get; set; }
        public double Balance { get; set; }
        [Required]
        public uint Type { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public Account Parent_Account { get; set; }
    }
}