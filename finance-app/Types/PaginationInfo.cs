using Microsoft.AspNetCore.Mvc;

namespace finance_app.Types {
    [ModelBinder(BinderType = typeof(PaginationInfoModleBinder))]
    public class PaginationInfo {
        
        public int? PageNumber { get; set; }

        public int? ItemsPerPage { get; set; }
    }
}
