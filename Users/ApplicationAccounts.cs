using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories.ApplicationAccounts
{
    [Table("application_accounts")]
    public class ApplicationAccount : DatabaseObject
    {
        /// <summary>
        /// The user's on the account
        /// </summary>
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}