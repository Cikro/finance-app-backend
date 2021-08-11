using System.Collections.Generic;
using System.Threading.Tasks;

using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.Services.V1
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountServiceDbo;
        public AccountService(IAccountRepository accountServiceDbo){
            _accountServiceDbo = accountServiceDbo;
        }

        public List<AccountDto> GetAccounts(uint userId){
            var accounts = _accountServiceDbo.GetAllByUserId(userId);
            var accountDtos = new List<AccountDto>();

            foreach(var a in accounts) {
                accountDtos.Add(new AccountDto
                {
                    Id = a.Id,
                    UserId = a.User_Id,
                    Balance = a.Balance,
                    Name = a.Name,
                    CurrencyCode = a.Currency_Code,
                    Description = a.Description,
                    ParentAccountId = a.Parent_Account_Id,
                    Type = new EnumDto<AccountTypeDtoEnum>(GetAccountType(a.Type)),
                    DateCreated = a.Date_Created,
                    DateLastEdited = a.Date_Last_Edited 
                });
            }
            return accountDtos;
        }
        public List<AccountDto> GetPaginatedAccounts(uint userId, PaginationInfo pageInfo)
        {
            if (pageInfo.PageNumber <= 0)
            {
                return null;
            }
            if (pageInfo.ItemsPerPage < 0)
            {
                return null;
            }

            uint offset = (uint)pageInfo.PageNumber - 1;
            
            var accounts = _accountServiceDbo.GetPaginatedByUserId(userId, (uint)pageInfo.ItemsPerPage, offset);
            var accountDtos = new List<AccountDto>();
            foreach (var a in accounts) { accountDtos.Add(MapAccountDto(a)); }

            return accountDtos;
        }

        public void InsertAccount(){

        }

        public void UpdateAccounts(){

        }
        public void DeleteAccounts(){

        }

        private AccountTypeDtoEnum GetAccountType(AccountTypeEnum accountType)
        {
            AccountTypeDtoEnum type = AccountTypeDtoEnum.Unknown;
            switch (accountType)
            {
                case AccountTypeEnum.Asset:
                    type = AccountTypeDtoEnum.Asset;
                break;
                case AccountTypeEnum.Liability:
                    type = AccountTypeDtoEnum.Liability;
                break;
                case AccountTypeEnum.Revenue:
                    type = AccountTypeDtoEnum.Revenue;
                break;
                case AccountTypeEnum.Expense:
                    type = AccountTypeDtoEnum.Expense;
                break;

            }
            return type;
        }
        private AccountDto MapAccountDto(Account account)
        {
            return (new AccountDto
            {
                Id = account.Id,
                UserId = account.User_Id,
                Balance = account.Balance,
                Name = account.Name,
                CurrencyCode = account.Currency_Code,
                Description = account.Description,
                ParentAccountId = account.Parent_Account_Id,
                Type = new EnumDto<AccountTypeDtoEnum>(GetAccountType(account.Type)),
                DateCreated = account.Date_Created,
                DateLastEdited = account.Date_Last_Edited
            });

        }

    }
}
