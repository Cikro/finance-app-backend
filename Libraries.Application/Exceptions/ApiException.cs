using System;
using System.Net;

namespace finance_app.Exceptions
{

    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string MyProperty { get; set; }
        public int ExceptionCode { get; set; }

    }
}