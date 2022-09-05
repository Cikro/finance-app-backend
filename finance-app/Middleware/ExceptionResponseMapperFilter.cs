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
using System;
using finance_app.Types.Services.V1.ResponseMessages;

namespace finance_app.Middleware
{
    public class ExceptionResponseMapperFilter : IExceptionFilter
    {

        private const int MAX_INNER_EXCEPTIONS = 5;

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
            
             var message = new ExceptionResponseMessage();
            var apiResponse = new ApiResponse<ExceptionDto>(ApiResponseCodesEnum.InternalError, message);
            
            if (_hostingEnvironment.IsDevelopment())
            {
                apiResponse.Data =  BuildExceptionDto(context.Exception);
            }


            context.Result = new JsonResult(apiResponse) {
                StatusCode = _mapper.Map<int>(apiResponse.ResponseCode)
            };
        }


        private ExceptionDto BuildExceptionDto(Exception dto, int count = 0) {
            if (dto == null || count >= MAX_INNER_EXCEPTIONS) { return null; }

            count++;
            return new ExceptionDto {
                Message = dto.Message,
                StackTrace = dto.StackTrace,
                InnerException = BuildExceptionDto(dto.InnerException, count)
            };
        }
    }
}
