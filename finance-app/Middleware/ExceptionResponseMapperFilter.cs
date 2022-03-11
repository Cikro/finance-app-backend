using System.Net;
using finance_app.Types;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using AutoMapper;

namespace finance_app.Middleware
{
    public class ExceptionResponseMapperFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly IMapper _mapper;

        public ExceptionResponseMapperFilter(IWebHostEnvironment hostingEnvironment,
                                            IModelMetadataProvider modelMetadataProvider, 
                                            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _mapper = mapper;
        }

        public void OnException(ExceptionContext context)
        {
            var message = "Exceptional Failure!";
            var apiResponse = new ApiResponse<ExceptionDto>(ApiResponseCodesEnum.InternalError, message);
            
            if (_hostingEnvironment.IsDevelopment())
            {
                apiResponse.Data = new ExceptionDto {
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace
                };
            }


            context.Result = new JsonResult(apiResponse) {
                StatusCode = _mapper.Map<int>(apiResponse.ResponseCode)
            };
        }
    }
}
