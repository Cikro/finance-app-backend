

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace finance_app.Types.ModleBinders {
    public class RequestModelBinder : ComplexTypeModelBinder {
        public RequestModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders, ILoggerFactory loggerFactory) : base(propertyBinders, loggerFactory) {}

        protected override object CreateModel(ModelBindingContext bindingContext) 
        {

            // If model type is registered for DI, let DI create the model.
            //object model = bindingContext.HttpContext.RequestServices.GetService(bindingContext.ModelType) ?? base.CreateModel(bindingContext);
            object model = base.CreateModel(bindingContext);
            return model;
            


        }
    }
}
