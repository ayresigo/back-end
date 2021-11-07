using back_end.Entities;
using back_end.InputModel;
using back_end.Services;
using back_end.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    public class UserAccountRepo : IUserAccountRepo
    {
        private readonly MySqlConnection conn;

        public UserAccountRepo(IConfiguration config)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<UserAccount> getAccount(string address)
        {
            UserAccount userAccount = null;
            var query = $"SELECT * FROM accounts WHERE address = '{address}'";
            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                userAccount = new UserAccount
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
            return userAccount;
        }
    }
}
