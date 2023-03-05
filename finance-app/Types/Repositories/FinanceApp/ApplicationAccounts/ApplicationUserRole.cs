using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace finance_app.Types.Repositories.ApplicationAccounts
{
    [Table("application_user_roles")]
    public class ApplicationUserRole : DatabaseObject
    {
        /// <summary>
        /// The Application user this account represents
        /// </summary>
        [Required]
        [Column("application_user_id")]
        public uint ApplicationUserId { get; set; }


        /// <summary>
        /// The Application User
        /// </summary>
        public ApplicationUser ApplicationUser{ get; set; }


        /// <summary>
        /// The id of the Role The User has
        /// </summary>
        [Required]
        [Column("role_id")]
        public uint ApplicationRoleId { get; set; }

        /// <summary>
        /// The ApplciationRole the user has
        /// </summary>
        public ApplicationRole ApplicationRole{ get; set; }
    }
}