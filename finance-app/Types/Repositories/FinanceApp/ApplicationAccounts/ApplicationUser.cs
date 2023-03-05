using finance_app.Types.Repositories.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;

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
        public IEnumerable<ApplicationUserRole> ApplicationUserRoles { get; set; }


        /// <summary>
        /// The Authentication User that controls this account
        /// </summary>
        [Required]
        [Column("authentication_user_id")]
        public uint AuthenticationUserId { get; set; }

        // TODO: Insead of using a concrete class... Might want to change input to an interface of type Aiuthetication user...
        public List<Claim> GetClaims(AuthenticationUser authenticationUser) 
        {
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Email, authenticationUser.AuthenticationUserInfo?.Email),
                new Claim(ClaimTypes.Name, authenticationUser.UserName),
            };

            claims.AddRange(ApplicationUserRoles.Select(x => new Claim(ClaimTypes.Role, x.ApplicationRoleId.ToString())));

            return claims;

        }
    }
}