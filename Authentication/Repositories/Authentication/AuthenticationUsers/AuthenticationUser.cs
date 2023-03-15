using finance_app.Types.Services.V1.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories.Authentication
{
    [Table("authentication_users")]
    internal class AuthenticationUser : DatabaseObject
    {
        /// <summary>
        /// The username used to authenticate the uer
        /// </summary>
        [Required]
        [Column("user_name")]
        public string UserName { get; set; }


        /// <summary>
        /// The User's password hash to login
        /// </summary>
        [Required]
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// The User's salty for their password
        /// </summary>
        [Required]
        [Column("password_salt")]
        public byte[] PasswordSalt{ get; set; }

        public AuthenticationUserInfo AuthenticationUserInfo { get; set; }


    }
}