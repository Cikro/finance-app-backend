using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories.Authentication
{
    [Table("authentication_users")]
    public class AuthenticationUser : DatabaseObject
    {
        /// <summary>
        /// The username used to authenticate the uer
        /// </summary>
        [Required]
        [Column("user_name")]
        public string UserName { get; set; }


        /// <summary>
        /// the User's password hash to login
        /// </summary>
        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

    }
}