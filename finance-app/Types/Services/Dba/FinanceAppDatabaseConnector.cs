using System;
using MySql.Data.MySqlClient;

namespace finance_app
{
    public class FinanceAppDatabase : IDisposable
    {
        public MySqlConnection Connection { get; }

        public FinanceAppDatabase(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}