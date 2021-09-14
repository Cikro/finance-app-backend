using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types
{
    public class PaginationInfoModleBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) { throw new ArgumentNullException(nameof(bindingContext)); }


            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var pageNumberProvider = bindingContext.ValueProvider.GetValue("pageNumber");
            var itemsPerPageProvider = bindingContext.ValueProvider.GetValue("itemsPerPage");

            if (pageNumberProvider == null && itemsPerPageProvider == null) {
                return Task.CompletedTask;
            }

            // properties to bind
            bindingContext.ModelState.SetModelValue("pageNumber", pageNumberProvider);
            bindingContext.ModelState.SetModelValue("itemsPerPage", itemsPerPageProvider);

            var pageNumber = pageNumberProvider.FirstValue;
            var itemsPerPage = itemsPerPageProvider.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(pageNumber) && string.IsNullOrEmpty(itemsPerPage)) {
                // Clear required to set ModelState.IsValid to true.
                // Otherwise it defaults to false and returns an Http 400 with no errors.
                bindingContext.ModelState.Clear();
                return Task.CompletedTask;
            }

            int? iPageNumber = null;
            int? iItemsPerPage = null;
            int parseVal;
            if (string.IsNullOrEmpty(pageNumber)) {
                iPageNumber = null;
            } else if (int.TryParse(pageNumber, out parseVal)) {
                iPageNumber = parseVal;
            } else {
                // Non-integer arguments result in model state errors
                bindingContext.ModelState.TryAddModelError(
                    modelName, "PageNumber must be an integer.");
            }

            if (string.IsNullOrEmpty(itemsPerPage)) {
                iItemsPerPage = null;
            } else if (int.TryParse(itemsPerPage, out parseVal)) { 
                iItemsPerPage = parseVal;
            } else {
                // Non-integer arguments result in model state errors
                bindingContext.ModelState.TryAddModelError(
                    modelName, "ItemsPerPage must be an integer.");
            }

            if (bindingContext.ModelState.ErrorCount >= 1)
            {
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(new PaginationInfo { 
                PageNumber = iPageNumber,
                ItemsPerPage = iItemsPerPage
            });

            // Clear required to set ModelState.IsValid to true.
            // Otherwise it defaults to false and returns an Http 400 with no errors.
            bindingContext.ModelState.Clear();
            return Task.CompletedTask;
        }


    }
}
