using back_end.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    public class AccountSQLRepo : IAccountRepository
    {
        private readonly MySqlConnection conn;

        public AccountSQLRepo(IConfiguration config)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
        }

        public async Task addAccount(Account account)
        {
            var query = $"INSERT accounts (id, address, username, ban) values ('{account.Id}', '{account.Address}', '{account.Username}', '{account.Ban}'";

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            sqlCommand.ExecuteNonQuery();
            await conn.CloseAsync();
        }

        public async Task banAccount(string address, bool ban = true)
        {
            var query = $"UPDATE accounts SET ban = '{ban}' WHERE address = '{address}";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public async Task banAccount(int id, bool ban = true)
        {
            var query = $"UPDATE accounts SET ban = '{ban}' WHERE id = '{id}";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public async Task deleteAccount(int id)
        {
            var query = $"DELETE FROM accounts WHERE id = '{id}";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public async Task deleteAccount(string address)
        {
            var query = $"DELETE FROM accounts WHERE address = '{address}";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task editAccount(int id, Account account)
        {
            var query = $"UPDATE accounts SET id='{account.Id}', address='{account.Address}', username='{account.Username}', ban='{account.Ban}' WHERE id = '{id}";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public async Task editAccount(string address, Account account)
        {
            var query = $"UPDATE accounts SET id='{account.Id}', address='{account.Address}', username='{account.Username}', ban='{account.Ban}' WHERE address = '{address}";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public async Task<List<Account>> getAccount(int page, int qtd)
        {
            var accounts = new List<Account>();
            var query = $"SELECT * FROM accounts ORDER BY id";

            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                accounts.Add(new Account
                {
                    Id = (int)sqlDataReader["Id"],
                    Address = (string)sqlDataReader["Address"],
                    Username = (string)sqlDataReader["Username"],
                    Ban = (bool)sqlDataReader["Ban"]
                });
            }

            await conn.CloseAsync();

            return accounts;
        }

        public async Task<Account> getAccount(int id)
        {
            Account account = null;
            var query = $"SELECT * FROM accounts WHERE id = '{id}'";

            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                account = new Account
                {
                    Id = (int)sqlDataReader["Id"],
                    Address = (string)sqlDataReader["Address"],
                    Username = (string)sqlDataReader["Username"],
                    Ban = (bool)sqlDataReader["Ban"]
                };
            }

            await conn.CloseAsync();

            return account;
        }

        public async Task<Account> getAccount(string address)
        {
            Account account = null;
            var query = $"SELECT * FROM accounts WHERE id = '{address}'";

            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                account = new Account
                {
                    Id = (int)sqlDataReader["Id"],
                    Address = (string)sqlDataReader["Address"],
                    Username = (string)sqlDataReader["Username"],
                    Ban = (bool)sqlDataReader["Ban"]
                };
            }

            await conn.CloseAsync();

            return account;
        }
    }
}
