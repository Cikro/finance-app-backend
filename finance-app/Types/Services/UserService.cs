using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.Interfaces;
using finance_app.Types.EFModels;
using finance_app.Types.Responses.Dtos;
using finance_app.Types.Responses.Dtos.Enums;
using finance_app.Types.Responses;

namespace finance_app.Types.Services
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
