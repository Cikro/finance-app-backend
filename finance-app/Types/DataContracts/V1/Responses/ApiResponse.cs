using System.Net;

namespace finance_app.Types.DataContracts.V1.Responses
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// The Http Status Code of the request
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// A number indicating a specific outocme or problem with the response
        /// </summary>
        public ApiResponseCodesEnum ResponseCode { get; set; }
        
        /// <summary>
        /// A human readable message explaining the response code to the user.
        /// </summary>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// The data in the response
        /// </summary>
        /// <value></value>
        public T Data{ get; set; }
    }
}
