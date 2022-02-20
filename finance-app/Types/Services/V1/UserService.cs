using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models;
using finance_app.Types.Repositories;
using finance_app.Types.Services.V1.Interfaces;

namespace finance_app.Types.Services.V1
{
    public class UserAuthorizationServiceService : IUserAuthorizationService
    {
        
        public UserAuthorizationServiceService(){
        }

        public bool CanAccessResource(uint resourceId, uint userId)
        {
            return userId == resourceId;
        }

        public bool CanAccessResource(DatabaseObject resource, uint userId)
        {
            return userId == resource.Id;
        }

    }
}
