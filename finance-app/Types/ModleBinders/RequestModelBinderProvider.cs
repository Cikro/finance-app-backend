

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace finance_app.Types.ModleBinders {
    public class RequestModelBinderProvider : IModelBinderProvider {

        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (context == null) { throw new ArgumentNullException(nameof(context));}

            //if (!(context.Metadata.ModelType is Requests.IRequest)) { return null; }
            var interfaces = (IEnumerable<Type>)(((dynamic)context.Metadata.ModelType).ImplementedInterfaces);
            if (!(interfaces?.Contains(typeof(Requests.IRequest))) == true) { return null; }


            var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>(); ;
            foreach (ModelMetadata property in context.Metadata.Properties) {
                propertyBinders.Add(property, context.CreateBinder(property));
            }

            var loggerFactory = (LoggerFactory) context.Services.GetService(typeof(ILoggerFactory));
            return new RequestModelBinder(propertyBinders, loggerFactory);
        }

            
    }
}
