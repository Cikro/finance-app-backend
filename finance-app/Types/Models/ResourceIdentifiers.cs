using Microsoft.AspNetCore.Mvc;

namespace finance_app.Types.Models {

    public interface UIntResourceIdentifier {

        public uint Id { get; set; }
    }

    public class UserResourceIdentifier : UIntResourceIdentifier {


        [FromRoute(Name ="userId")]
        public uint Id { get; set; }
    }
    public class AccountResourceIdentifier : UIntResourceIdentifier {

        [FromRoute(Name ="accountId")]
        public uint Id { get; set; }
    }
}