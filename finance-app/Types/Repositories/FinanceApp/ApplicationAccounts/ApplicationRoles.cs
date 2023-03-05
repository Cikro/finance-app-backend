using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace finance_app.Types.Repositories.ApplicationAccounts
{
    [Table("application_roles")]
    public class ApplicationRole: DatabaseObject
    {
        /// <summary>
        /// The Application user this account represents
        /// </summary>
        [Required]
        [Column("role_name")]
        public string RoleName{ get; set; }
        
    }
}