using System.Net;
using finance_app.Types;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;

namespace finance_app.Middleware
{
    public class ExceptionResponseMapperFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public ExceptionResponseMapperFilter( IWebHostEnvironment hostingEnvironment,
                                            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            ApiResponse<ExceptionDto> apiResponse = new ApiResponse<ExceptionDto>
            {
                ResponseCode = ApiResponseCodesEnum.InternalError,
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseMessage = "Exceptional Failure!",
            };
            
            if (_hostingEnvironment.IsDevelopment())
            {
                apiResponse.Data = new ExceptionDto {
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace
                };
            }


            context.Result = new JsonResult(apiResponse) {
                StatusCode = (int) apiResponse.StatusCode
            };
        }
    }
}
