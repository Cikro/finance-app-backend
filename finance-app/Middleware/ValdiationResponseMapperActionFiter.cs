using System.Collections.Generic;
using System.Linq;
using finance_app.Types;
using finance_app.Types.Responses;
using finance_app.Types.Responses.Dtos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Filters;

namespace finance_app.Middleware {
    public class ValidationResponseMapperFilter : IActionFilter {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) { return; }

            var errors = context.ModelState
                .Where(v => v.Value.Errors.Count > 0)
                .Select(v => new ValidationError
                {
                    Key = v.Key,
                    Errors = v.Value.Errors.Select(v => v.ErrorMessage).ToList() 
                });
            var apiResponse = new ApiResponse<List<ValidationError>> {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ResponseCode = ApiResponseCodesEnum.BadRequest,
                ResponseMessage = "There are errors in your input parameters",
                Data = errors.ToList()
            };

            context.Result = new JsonResult(apiResponse) {
                StatusCode = (int) apiResponse.StatusCode
            };
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

    }
}