using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using finance_app.DataAccessObjects;

namespace finance_app.Services
{
    
    public class AccountsDbaService : IAccountsDbaService
    {
        FinanceAppDatabase _Db;
        AccountsDbaService(FinanceAppDatabase db) {
            _Db = db;

        }

        public async Task<List<Account>> GetAccounts()
        {
            using var cmd = _Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `BlogPost` (`Title`, `Content`) VALUES (@title, @content);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int) cmd.LastInsertedId;

            return new List<Account>();
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = Title,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@content",
                DbType = DbType.String,
                Value = Content,
            });
        }

        
    }
}
