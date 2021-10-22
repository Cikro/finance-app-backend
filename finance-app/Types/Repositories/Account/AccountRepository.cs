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

        public async Task<Account> GetAccountByAccountId(uint accountId) {
            Account account = null;

            var parameters = new object[]
            {
                new MySqlParameter("accountId",accountId)
            };

            var connection = _context.Database.GetDbConnection();
            
            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetAccountByAccountId";
                foreach (var p in parameters) {
                    command.Parameters.Add(p);

                }
    
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        account = ReadAccount(reader);
                    }
                }
                await connection.CloseAsync();


            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            } 

            return account;
        }

        public async Task<Account> GetAccountByAccountName(uint userId, string accountName) {
            
            Account account = null;
            
            var parameters = new object[] {
                new MySqlParameter("userId", userId),
                new MySqlParameter("accountName", accountName)
            };

            var connection = _context.Database.GetDbConnection();
            
            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetAccountByAccountName";
                foreach (var p in parameters) {
                    command.Parameters.Add(p);

                }

                
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        account = ReadAccount(reader);
                    }
                }
                await connection.CloseAsync();


            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            } 
            return account;
        }

        public async Task<List<Account>> GetAllByUserId(uint userId) {
            
            var accounts = new List<Account>();

            var parameters = new object[] {
                new MySqlParameter("userId",userId)
            };

            var connection = _context.Database.GetDbConnection();
            
            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetAllAccountsByUserId";
                foreach (var p in parameters) {
                    command.Parameters.Add(p);

                }

                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        accounts.Add(ReadAccount(reader));
                    }
                }
                await connection.CloseAsync();

            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            } 

            return accounts;
        }

        public async Task<List<Account>> GetPaginatedByUserId(uint userId, uint pageSize, uint offset)
        {
            var accounts = new List<Account>();

            var totalItems = new MySqlParameter("totalItems", MySqlDbType.UInt32, 4) {
                Direction = ParameterDirection.Output
            };
            var parameters = new object[]
            {
                new MySqlParameter("userId",userId),
                new MySqlParameter("itemsPerPage",pageSize),
                new MySqlParameter("pageOffset",offset),
                totalItems
                
            };
            
            var connection = _context.Database.GetDbConnection();

            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetAccountsByUserId";

                foreach (var p in parameters) {
                    command.Parameters.Add(p);
                }

                
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        accounts.Add(ReadAccount(reader));
                    }
                }
                await connection.CloseAsync();

            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            }

            return accounts;
        }
        public async Task<List<Account>> GetChildrenByAccountId(uint accountId) {
            var accounts = new List<Account>();

            var parameters = new object[] {
                new MySqlParameter("accountId", accountId)
            };

            var connection = _context.Database.GetDbConnection();
            
            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetChildrenByAccountId";
                foreach (var p in parameters) {
                    command.Parameters.Add(p);

                }

                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        accounts.Add(ReadAccount(reader));
                    }
                }
                await connection.CloseAsync();

            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            } 

            return accounts;
        }


        public async Task<Account> CreateAccount(Account account) {
            Account newAccount = null;

            var parameters = new object[] {
                new MySqlParameter("userId", account.User_Id),
                new MySqlParameter("accountName", account.Name),
                new MySqlParameter("accountDescription", account.Description),
                new MySqlParameter("accountType", account.Type),
                new MySqlParameter("currencyCode", account.Currency_Code),
                new MySqlParameter("parentAccountId", account.Parent_Account_Id)
            };
            
            var connection = _context.Database.GetDbConnection();

            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CreateAccount";

                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }

                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        newAccount = ReadAccount(reader);
                    }
                }
                await connection.CloseAsync();
            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            }

            return newAccount;
        }

        public async Task<List<Account>> CloseAccount(uint accountId) {
            var accountsClosed = new List<Account>();;

            var parameters = new object[]
            {
                new MySqlParameter("accountId", accountId),
            };
            
            var connection = _context.Database.GetDbConnection();

            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CloseAccountByAccountId";

                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }

                
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        accountsClosed.Add(ReadAccount(reader));
                    }
                }
                await connection.CloseAsync();

            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            }

            return accountsClosed;

        }

        public async Task<Account> UpdateAccount(Account account) {
            Account updatedAccount = null;

            var parameters = new object[]
            {
                new MySqlParameter("accountId", account.Id),
                new MySqlParameter("accountName", account.Name),
                new MySqlParameter("accountDescription", account.Description),
                new MySqlParameter("accountClosed", account.Closed),
            };
            
            var connection = _context.Database.GetDbConnection();

            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateAccount";

                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }

                
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        updatedAccount = ReadAccount(reader);
                    }
                }
                await connection.CloseAsync();

            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            }

            return updatedAccount;

        }

        public void UpdateItem(Account account) {
            _context.Update(account);
        }

        private Account ReadAccount(DbDataReader reader)
        {
            return new Account
            {
                Id = (uint)reader.GetInt32("id"),
                User_Id = (uint)reader.GetInt32("user_id"),
                Name = reader.IsDBNull("name") ? "" : reader.GetString("name"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                Balance = reader.IsDBNull("balance") ? 0 : reader.GetDecimal("balance"),
                Type = Enum.IsDefined(typeof(AccountTypeEnum), reader.GetByte("type")) ? (AccountTypeEnum) reader.GetByte("type") : AccountTypeEnum.Unknown,
                Currency_Code = reader.IsDBNull("currency_code") ? "" : reader.GetString("currency_code"),
                Parent_Account_Id = reader.IsDBNull("parent_account") ? null : (uint?)reader.GetInt32("parent_account"),
                Closed = reader.IsDBNull("closed") ? null : (bool?)reader.GetBoolean("closed"),
                Date_Created = reader.GetDateTime("date_created"),
                Date_Last_Edited = reader.GetDateTime("date_last_edited")
            };

        }

    }
}
