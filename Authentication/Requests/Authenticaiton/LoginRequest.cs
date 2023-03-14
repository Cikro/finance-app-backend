namespace finance_app.Types.DataContracts.V1.Requests.Authentication
{
    public class LoginRequest {
        
        /// <summary>
        /// The user that is being created
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password that for that user
        /// </summary>
        public string Password { get; set; }


    }
}