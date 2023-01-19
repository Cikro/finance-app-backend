using System.Collections.Generic;

namespace finance_app.Types.DataContracts.V1.Dtos
{
    public class AuthenticationUserDto : BaseDto
    {
        /// <summary>
        /// the User's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The User's email
        /// </summary>
        public string Email { get; set; }

    }
}
