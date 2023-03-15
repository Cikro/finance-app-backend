using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace finance_app.Types.Repositories.Authentication
{
    [Table("authentication_users_info")]
    internal class AuthenticationUserInfo : DatabaseObject
    {
        /// <summary>
        /// The Authentication User's email
        /// </summary>
        [Required]
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// Foreign Key to Authentication Users 
        /// </summary>
        [Required]
        [Column("authentication_user_id")]
        public uint AuthenticationUserId { get; set; }

        /// <summary>
        /// Foreign Key to Authentication Users 
        /// </summary>
        public AuthenticationUser AuthenticationUser { get; set; }

        

    }
}