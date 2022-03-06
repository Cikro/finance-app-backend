using System.Collections.Generic;
using System.Linq;
using finance_app.Types;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
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

            var message = "There are errors in your input parameters";
            var apiResponse = new ApiResponse<List<ValidationError>>(ApiResponseCodesEnum.BadRequest, message);

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