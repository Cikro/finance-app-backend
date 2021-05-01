using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.EFModels
{
    [Table("accounts")]
    public class Account : DatabaseObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public uint User_Id { get; set; }
        public string Description { get; set; }
        public double Balance { get; set; }
        [Required]
        public uint Type { get; set; }
        [Required] 
        public string Currency_Code { get; set; }
        public uint? Parent_Account_Id { get; set; }
    }
}