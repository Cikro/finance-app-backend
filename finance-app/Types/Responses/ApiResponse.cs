using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types
{
    public class ApiResponse<T>
    {

        public string ResponseMessage { get; set; }
        public T Data{ get; set; }
    }
}
