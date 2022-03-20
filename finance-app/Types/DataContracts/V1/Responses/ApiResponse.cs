using System.Collections.Generic;
using System.Net;

namespace finance_app.Types.DataContracts.V1.Responses
{
    public class ApiResponse<T>
    {
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

        public ApiResponse(T data)
        {
            Data = data;
            ResponseCode = ApiResponseCodesEnum.Success;
            ResponseMessage = "Successful";
        }

        public ApiResponse(ApiResponseCodesEnum responseCode, string message)
        {
            Data = default;
            ResponseCode = responseCode;
            ResponseMessage = message;
        }

        public ApiResponse(T data, ApiResponseCodesEnum responseCode, string message)
        {
            Data = data;
            ResponseCode = responseCode;
            ResponseMessage = message;
        }
    }
}
