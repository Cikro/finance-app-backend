using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class PostAccountRequest
    {
        public uint Id;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Closed { get; set; }

    }
}
