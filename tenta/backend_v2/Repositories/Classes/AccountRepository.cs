using cryminals.Exceptions;
using cryminals.Models.ViewModels;
using cryminals.Repositories.Interfaces;
using cryminals.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Classes
{
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

        public async Task<List<CharacterViewModel>> fetchCharacters(string token)
        {
            try
            {
                var account = _authService.retrieveTokenData(token);
                var characters = new List<CharacterViewModel>();
                var query = $"SELECT * FROM `characters` INNER JOIN `character_status`  " +
                    $"ON `characters`.`fk_status_id` = `character_status`.`status_id` " +
                    $"WHERE `characters`.`fk_owner_address` = '{account.address}'";

                await conn.OpenAsync();
                MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                while (sqlDataReader.Read())
                {
                    var status = new CharacterStatusViewModel
                    {
                        Id = (int)sqlDataReader["status_id"],
                        Name = (string)sqlDataReader["status_name"],
                        Icon = (string)sqlDataReader["icon"],
                        IconColor = (string)sqlDataReader["iconColor"],
                        BgColor = (string)sqlDataReader["bgColor"],
                        Description = (string)sqlDataReader["description"],
                        statusDuration = (int)sqlDataReader["status_duration"],
                        statusChanged = (long)sqlDataReader["status_changed"],
                    };

                    characters.Add(new CharacterViewModel
                    {
                        Id = (int)sqlDataReader["id"],
                        Owner = (string)sqlDataReader["fk_owner_address"],
                        Name = (string)sqlDataReader["name"],
                        Gender = (string)sqlDataReader["gender"],
                        Seed = (long)sqlDataReader["seed"],
                        Rarity = (string)sqlDataReader["rarity"],
                        Power = (int)sqlDataReader["power"],
                        Health = (int)sqlDataReader["health"],
                        CurrentHealth = (int)sqlDataReader["currentHealth"],
                        Stamina = (int)sqlDataReader["stamina"],
                        CurrentStamina = (int)sqlDataReader["currentStamina"],
                        Job = (string)sqlDataReader["job"],
                        Affiliation = (string)sqlDataReader["affiliation"],
                        Status = status,
                    });
                }

                await conn.CloseAsync();
                return characters;
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
                var query = $"SELECT * FROM accounts WHERE address = '{address}'";

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

        public Task<List<CharacterViewModel>> getCharacters(string address)
        {
            throw new NotImplementedException();
        }
    }
}
