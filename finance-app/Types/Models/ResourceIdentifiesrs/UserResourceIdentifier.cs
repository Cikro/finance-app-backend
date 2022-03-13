using Microsoft.AspNetCore.Mvc;


namespace finance_app.Types.Models.ResourceIdentifiers {
    public class UserResourceIdentifier : UIntResourceIdentifier 
    {
        [FromRoute(Name ="userId")]
        public uint Id { get; set; }

        public UserResourceIdentifier(){}

        public UserResourceIdentifier(uint id) 
        {
            Id = id;
        }
    }

}