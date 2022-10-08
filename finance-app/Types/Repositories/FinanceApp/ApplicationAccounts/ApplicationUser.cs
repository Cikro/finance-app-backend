using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace finance_app.Types.Repositories.ApplicationAccounts
{
    [Table("application_users")]
    public class ApplicationUser : DatabaseObject
    {
        /// <summary>
        /// The Application user this account represents
        /// </summary>
        [Required]
        [Column("application_account_id")]
        public uint ApplicationAccountId { get; set; }

        /// <summary>
        /// The Application Account
        /// </summary>
        public ApplicationAccount ApplicationAccount { get; set; }


        /// <summary>
        /// The Authentication User that controls this account
        /// </summary>
        [Required]
        [Column("authentication_user_id")]
        public string AuthenticationUserId { get; set; }
    }
}