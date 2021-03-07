using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using finance_app.Types.EFModels;
using finance_app.Types.Interfaces;
using finance_app.Types.EFContexts;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;

namespace finance_app.Types.Services
{
    public class AccountServiceDbo: IAccountServiceDbo
    {
        private readonly AccountContext _context;
        public AccountServiceDbo(AccountContext context){
            _context = context;
        }

        private IQueryable<Account> GetQueryable() {
            return _context.Accounts.AsQueryable();

        }

        public IEnumerable<Account> GetAllByUserId(uint userId) {
            
            var p = new object[]
            {
                new MySqlParameter("userId",userId)
            };
            var connection = _context.Database.GetDbConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetAllAccountsByUserId";
            command.Parameters.Add(new MySqlParameter("@userId", userId));

            var reader = command.ExecuteReader();

            var accounts = new List<Account>();
            while (reader.Read()) {
                accounts.Add(new Account{
                    User_Id = (uint) reader.GetInt32("user_id"),
                    Name = reader.IsDBNull("name") ? "" : reader.GetString("name"),
                    Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                    Balance= reader.IsDBNull("balance") ? 0 : reader.GetDouble("balance"),
                    Type = (uint) reader.GetInt32("type"),
                    Currency_Code = reader.IsDBNull("currency_code") ? "" : reader.GetString("currency_code")
                });
            }
            connection.Close();



            return accounts;

            //return _context.Accounts.Where(i => i.User_Id == userId).ToList();
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

    }
}
