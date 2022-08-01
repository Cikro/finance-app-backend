using Microsoft.AspNetCore.Mvc;

namespace finance_app.Types.DataContracts.V1.Dtos {
    [ModelBinder(BinderType = typeof(PaginationInfoModelBinder))]
    public class PaginationInfo {
        
        public int? PageNumber { get; set; }

        public int? ItemsPerPage { get; set; }
    }
}
