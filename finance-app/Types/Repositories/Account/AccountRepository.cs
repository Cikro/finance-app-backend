using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using finance_app.Types.Repositories.Account;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace finance_app.Types.Repositories.Account
{
    public class AccountRepository: IAccountRepository
    {
        private readonly AccountContext _context;
        public AccountRepository(AccountContext context){
            _context = context;
        }

        private IQueryable<Account> GetQueryable() {
            return _context.Accounts.AsQueryable();

        }

        public List<Account> GetAllByUserId(uint userId) {
            
            var parameters = new object[]
            {
                new MySqlParameter("userId",userId)
            };
            var connection = _context.Database.GetDbConnection();
            
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetAllAccountsByUserId";
            foreach (var p in parameters)
            {
                command.Parameters.Add(p);

            }

            var reader = command.ExecuteReader();            
            var accounts = new List<Account>();
            while (reader.Read()) {
                accounts.Add(ReadAccount(reader));
            }
            connection.Close();

            return accounts;
        }
        public List<Account> GetPaginatedByUserId(uint userId, uint pageSize, uint offset)
        {
            var totalItems = new MySqlParameter("totalItems", MySqlDbType.UInt32, 4);
            totalItems.Direction = ParameterDirection.Output;
            var parameters = new object[]
            {
                new MySqlParameter("userId",userId),
                new MySqlParameter("itemsPerPage",pageSize),
                new MySqlParameter("pageOffset",offset),
                totalItems
                
            };
            

            var connection = _context.Database.GetDbConnection();

            connection.Open();
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetAccountsByUserId";

            foreach (var p in parameters)
            {
                command.Parameters.Add(p);

            }

            var reader = command.ExecuteReader();
            var accounts = new List<Account>();
            
            while (reader.Read())
            {
                accounts.Add(ReadAccount(reader));
            }
            connection.Close();

            var totalAccounts = totalItems.Value;

            return accounts;
        }


        public async Task CreateItem(Account account) {
            await _context.AddAsync(account);
        }

        public Account DeleteItem(int accountId) {
            _context.Remove(accountId);
            return new Account();

        }

        public void UpdateItem(Account account) {
            _context.Update(account);
        }

        private Account ReadAccount(DbDataReader reader)
        {
            return new Account
            {
                User_Id = (uint)reader.GetInt32("user_id"),
                Name = reader.IsDBNull("name") ? "" : reader.GetString("name"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                Balance = reader.IsDBNull("balance") ? 0 : reader.GetDecimal("balance"),
                Type = Enum.IsDefined(typeof(AccountTypeEnum), reader.GetByte("type")) ? (AccountTypeEnum) reader.GetByte("type") : AccountTypeEnum.Unknown,
                Currency_Code = reader.IsDBNull("currency_code") ? "" : reader.GetString("currency_code"),
                Parent_Account_Id = reader.IsDBNull("parent_account") ? null : (uint?)reader.GetInt32("parent_account"),
                Date_Created = reader.GetDateTime("date_created"),
                Date_Last_Edited = reader.GetDateTime("date_last_edited")
            };

        }

    }
}
