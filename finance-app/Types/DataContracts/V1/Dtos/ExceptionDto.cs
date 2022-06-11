using System;

namespace finance_app.Types.DataContracts.V1.Dtos {
    public class ExceptionDto {
        public string Message { get; set; }
        public string StackTrace { get; set; }  
        public ExceptionDto InnerException { get; set; }
    }
}