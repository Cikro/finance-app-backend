using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace finance_app.Types.Repositories.Accounts
{
    public class AccountRepository: IAccountRepository
    {
        private readonly AccountContext _context;
        public AccountRepository(AccountContext context){
            _context = context;
        }

        public async Task<Account> GetAccountByAccountId(uint? accountId) {
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


            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
            } 

            return account;
        }

        public async Task<Account> GetAccountByAccountName(uint? userId, string accountName) {
            
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


            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
            } 
            return account;
        }

        public async Task<IEnumerable<Account>> GetAllByUserId(uint? userId) {
            
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

            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
            } 

            return accounts;
        }

        public async Task<IEnumerable<Account>> GetPaginatedByUserId(uint? userId, uint pageSize, uint offset)
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

            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
            }

            return accounts;
        }
        public async Task<IEnumerable<Account>> GetChildrenByAccountId(uint? accountId) {
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

            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
            } 

            return accounts;
        }


        public async Task<Account> CreateAccount(Account account) {
            Account newAccount = null;

            var parameters = new object[] {
                new MySqlParameter("userId", account.UserId),
                new MySqlParameter("accountName", account.Name),
                new MySqlParameter("accountDescription", account.Description),
                new MySqlParameter("accountType", account.Type),
                new MySqlParameter("currencyCode", account.CurrencyCode),
                new MySqlParameter("parentAccountId", account.ParentAccountId)
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
            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
            }

            return newAccount;
        }

        public async Task<IEnumerable<Account>> CloseAccount(uint? accountId) {
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

            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
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

            } catch (Exception) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw;
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
                UserId = (uint)reader.GetInt32("user_id"),
                Name = reader.IsDBNull("name") ? "" : reader.GetString("name"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                Balance = reader.IsDBNull("balance") ? 0 : reader.GetDecimal("balance"),
                Type = Enum.IsDefined(typeof(AccountTypeEnum), reader.GetByte("type")) ? (AccountTypeEnum) reader.GetByte("type") : AccountTypeEnum.Unknown,
                CurrencyCode = reader.IsDBNull("currency_code") ? "" : reader.GetString("currency_code"),
                ParentAccountId = reader.IsDBNull("parent_account") ? null : (uint?)reader.GetInt32("parent_account"),
                Closed = reader.IsDBNull("closed") ? null : (bool?)reader.GetBoolean("closed"),
                DateCreated = reader.GetDateTime("date_created"),
                DateLastEdited = reader.GetDateTime("date_last_edited")
            };

        }

    }
}
