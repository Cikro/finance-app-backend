using System.Collections.Generic;
using System.Threading.Tasks;

using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using AutoMapper;

namespace finance_app.Types.Services.V1
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountServiceDbo;
        private readonly IMapper _mapper;
        

        public AccountService(IMapper mapper, IAccountRepository accountServiceDbo){

            _accountServiceDbo = accountServiceDbo;
            _mapper = mapper;
            
        }

        public List<AccountDto> GetAccounts(uint userId){
            var accounts = _accountServiceDbo.GetAllByUserId(userId);
            return new List<AccountDto>(_mapper.Map<List<AccountDto>>(accounts));
        }
        
        public List<AccountDto> GetPaginatedAccounts(uint userId, PaginationInfo pageInfo)
        {
            if (pageInfo.PageNumber <= 0) { return null; }
            if (pageInfo.ItemsPerPage < 0) { return null; }

            uint offset = (uint)pageInfo.PageNumber - 1;
            
            var accounts = _accountServiceDbo.GetPaginatedByUserId(userId, (uint)pageInfo.ItemsPerPage, offset);

            return new List<AccountDto>(_mapper.Map<List<AccountDto>>(accounts));
        }

        public void InsertAccount(){

        }

        public void UpdateAccounts(){

        }
        public void DeleteAccounts(){

        }
    }
}
