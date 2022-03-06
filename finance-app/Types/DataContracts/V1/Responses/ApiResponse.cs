using System.Collections.Generic;
using System.Net;

namespace finance_app.Types.DataContracts.V1.Responses
{
    public class ApiResponse<T>
    {

        private static Dictionary<ApiResponseCodesEnum, HttpStatusCode> StatusCodes 
            = new Dictionary<ApiResponseCodesEnum, HttpStatusCode> {
                { ApiResponseCodesEnum.Success, HttpStatusCode.OK },
                { ApiResponseCodesEnum.BadRequest, HttpStatusCode.BadRequest },
                { ApiResponseCodesEnum.InternalError, HttpStatusCode.InternalServerError },
                { ApiResponseCodesEnum.Unauthorized, HttpStatusCode.Unauthorized },
                { ApiResponseCodesEnum.DuplicateResource, HttpStatusCode.Conflict },
                { ApiResponseCodesEnum.ResourceNotFound, HttpStatusCode.Conflict }
            };
        /// <summary>
        /// The Http Status Code of the request
        /// </summary>
        public int StatusCode { get; set; }

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
            StatusCode = (int) StatusCodes[ApiResponseCodesEnum.Success];
            ResponseMessage = "Successful";
        }

        public ApiResponse(ApiResponseCodesEnum responseCode, string message)
        {
            Data = default;
            ResponseCode = responseCode;
            StatusCode = (int) StatusCodes[responseCode];
            ResponseMessage = message;
        }

        public ApiResponse(T data, ApiResponseCodesEnum responseCode, string message)
        {
            Data = data;
            ResponseCode = responseCode;
            StatusCode = (int) StatusCodes[responseCode];
            ResponseMessage = message;
        }
    }
}
