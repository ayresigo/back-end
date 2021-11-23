using back_end.Entities;
using back_end.InputModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private readonly MySqlConnection conn;

        public AccountRepo(IConfiguration config)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
        }
        public async Task<bool> createAccount(string address)
        {
            var query = $"INSERT INTO `accounts`(`address`) VALUES ('{address}')";

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            sqlCommand.ExecuteNonQuery();
            await conn.CloseAsync();
            return true;
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<bool> editUsername(string address, string username)
        {
            var account = await getAccount(address);
            if (account != null)
            {
                var query = $"UPDATE accounts SET username='{username}' WHERE address='{address}'";

                await conn.OpenAsync();
                MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                await conn.CloseAsync();
                return true;
            }
            else return false;
        }

        public async Task<Account> getAccount(string address)
        {
            Account account = null;
            var query = $"SELECT * FROM accounts WHERE address = '{address}'";

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
                    Avatar = (string)sqlDataReader["Avatar"],
                    Money = (int)sqlDataReader["Money"],
                    Respect = (int)sqlDataReader["Respect"],
                    TotalPower = (int)sqlDataReader["Total_Power"],
                    Status = (int)sqlDataReader["Status"]
                };
            }

            await conn.CloseAsync();
            return account;
        }

    }
}
