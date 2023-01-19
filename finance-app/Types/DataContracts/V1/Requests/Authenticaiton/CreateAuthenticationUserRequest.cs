namespace finance_app.Types.DataContracts.V1.Requests.Authentication
{
    public class CreateAuthenticationUserRequest {
        
        /// <summary>
        /// The user who is logging in
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The user's email
        /// </summary>
        public string Email { get; set; }


    }
}