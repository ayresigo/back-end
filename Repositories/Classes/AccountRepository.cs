using cryminals.Exceptions;
using cryminals.Models.ViewModels;
using cryminals.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Classes
{
    public interface IAccountRepository : IDisposable
    {
        Task<AccountViewModel> getAccount(string address);
        Task<AccountViewModel> fetchAccount(string token);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;
        private readonly IAuthService _authService;
        public AccountRepository(IConfiguration config, ICheckInputs checkInputs, IAuthService authService)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _checkInputs = checkInputs;
            _authService = authService;
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<AccountViewModel> fetchAccount(string token)
        {
            try
            {
                var tokenData = _authService.retrieveTokenData(token);
                return await getAccount(tokenData.address);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<AccountViewModel> getAccount(string address)
        {
            if (_checkInputs.checkAddress(address))
            {
                AccountViewModel account = null;
                var query = $"SELECT * FROM `accounts` WHERE `address` = '{address}'";

                await conn.OpenAsync();

                MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                while (sqlDataReader.Read())
                {
                    account = new AccountViewModel
                    {
                        Address = (string)sqlDataReader["Address"],
                        Username = (string)sqlDataReader["Username"],
                        Avatar = (string)sqlDataReader["Avatar"],
                        Money = (int)sqlDataReader["Money"],
                        Respect = (int)sqlDataReader["Respect"],
                        Status = (int)sqlDataReader["Status"],
                        CreationDate = (long)sqlDataReader["CreationDate"],
                    };
                }

                await conn.CloseAsync();
                return account;
            }
            else throw new InvalidInputException("address");
        }
    }
}
