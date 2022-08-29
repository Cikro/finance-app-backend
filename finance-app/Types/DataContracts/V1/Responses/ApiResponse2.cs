using System.Collections.Generic;
using System.Net;
using finance_app.Types.DataContracts.V1.Responses.ResponseMessage;

namespace finance_app.Types.DataContracts.V1.Responses
{
    public class ApiResponse2<T>
    {
        /// <summary>
        /// A number indicating a specific outcome or problem with the response
        /// </summary>
        public ApiResponseCodesEnum ResponseCode { get; set; }
        
        /// <summary>
        /// A human readable message explaining the response code to the user.
        /// </summary>
        private IResponseMessage _responseMessage { get; set; }
        public string ResponseMessage { get => _responseMessage?.GetMessage() ?? "" ; }

        /// <summary>
        /// The data in the response
        /// </summary>
        /// <value></value>
        public T Data{ get; set; }

        public ApiResponse2(T data)
        {
            Data = data;
            ResponseCode = ApiResponseCodesEnum.Success;
            _responseMessage = new SuccessResponseMessage();
        }

        public ApiResponse2(ApiResponseCodesEnum responseCode, IResponseMessage message)
        {
            Data = default;
            ResponseCode = responseCode;
            _responseMessage = message;
        }

        public ApiResponse2(T data, ApiResponseCodesEnum responseCode, IResponseMessage message)
        {
            Data = data;
            ResponseCode = responseCode;
            _responseMessage = message;
        }
    }
}
