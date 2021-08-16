using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.Services.V1.Interfaces;

namespace finance_app.Types.Services.V1
{
    public class UserService : IUserService
    {
        
        public UserService(){
        }

        public async Task<bool> CanAccessUser(uint userId, uint userIdToAccess)
        {
            return true;
        }

    }
}
