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
    public class CharacterRepository : ICharacterRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;
        private readonly IAuthService _authService;

        public CharacterRepository(IConfiguration config, ICheckInputs checkInputs, IAuthService authService)
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

        public async Task<string> getOwner(int id)
        {
            var isOwner = false;
            if (_checkInputs.checkInt(id))
            {

                var query = $"SELECT `fk_owner_address` FROM `characters` WHERE `id` = '{id}'";

                await conn.OpenAsync();
                MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                while (sqlDataReader.Read())
                {
                    var _address = (string)sqlDataReader["fk_owner_address"];
                    return _address;
                }
                await conn.CloseAsync();
                return null;
            }
            else
            {
                throw new InvalidInputException("address");
            }
        }

        public async Task changeStatus(int characterId, int status, int duration)
        {
            try
            {
                if (_checkInputs.checkInt(characterId) && _checkInputs.checkInt(duration) && _checkInputs.checkInt(status))
                {

                    var query = $"UPDATE `characters` SET `fk_status_id`={status}, `status_duration`={duration}, `status_changed`={DateTimeOffset.Now.ToUnixTimeSeconds()} WHERE `id`={characterId}";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                    await conn.CloseAsync();
                }
                else throw new InvalidInputException("input");
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }


        public async Task editCurrentStat(int characterId, string stat, int amount)
        {
            try
            {
                if (_checkInputs.checkInt(characterId) && _checkInputs.checkInt(amount))
                {
                    string currentStat = null;
                    switch (stat)
                    {
                        case "health":
                            currentStat = "currentHealth";
                            break;
                        case "stamina":
                            currentStat = "currentStamina";
                            break;
                        default:
                            throw new InvalidInputException("stat");

                    }
                    var query = $"UPDATE `characters` SET `{currentStat}`={amount} WHERE `id`={characterId}";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                    await conn.CloseAsync();
                }
                else throw new InvalidInputException("id or amount");
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
                if (_checkInputs.checkToken(token))
                {
                    var account = _authService.retrieveTokenData(token);
                    var characterIds = new List<int>();
                    var characters = new List<CharacterViewModel>();

                    var query = $"SELECT `id` FROM `characters` WHERE `fk_owner_address` = '{account.address}'";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {

                        characterIds.Add((int)sqlDataReader["id"]);
                    }

                    await conn.CloseAsync();

                    foreach (int characterId in characterIds)
                    {
                        characters.Add(await getCharacter(characterId));
                    }
                    return characters;
                }
                else
                {
                    throw new InvalidInputException("address");
                }

            }
            catch (InvalidCastException err)
            {
                throw new InvalidCastException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            //    var account = _authService.retrieveTokenData(token);
            //    var characters = new List<CharacterViewModel>();
            //    var query = $"SELECT * FROM `characters` INNER JOIN `character_status`  " +
            //        $"ON `characters`.`fk_status_id` = `character_status`.`status_id` " +
            //        $"WHERE `characters`.`fk_owner_address` = '{account.address}'";
        }

        public async Task<CharacterViewModel> getCharacter(int id)
        {
            try
            {
                if (_checkInputs.checkInt(id))
                {
                    CharacterViewModel character = null;
                    var query = $"SELECT * FROM `characters` INNER JOIN `character_status`  " +
                    $"ON `characters`.`fk_status_id` = `character_status`.`status_id` " +
                    $"WHERE `characters`.`id` = '{id}'";

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

                        character = new CharacterViewModel
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
                        };
                    }

                    await conn.CloseAsync();

                    if (character.Status.Id != 1)
                    {
                        if (character.Status.statusChanged + character.Status.statusDuration <= DateTimeOffset.Now.ToUnixTimeSeconds())
                        {
                            await changeStatus(character.Id, 1, 1);
                        }
                    }

                    return character;
                }
                else
                {
                    throw new InvalidInputException("id");
                }
            }
            catch (InvalidInputException err)
            {
                throw new InvalidInputException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<List<CharacterViewModel>> getCharacters(string address)
        {
            try
            {
                if (_checkInputs.checkAddress(address))
                {

                    var characterIds = new List<int>();
                    var characters = new List<CharacterViewModel>();

                    var query = $"SELECT `id` FROM `characters` WHERE `fk_owner_address` = '{address}'";

                    //var characters = new List<CharacterViewModel>();
                    //var query = $"SELECT * FROM `characters` INNER JOIN `character_status`  " +
                    //$"ON `characters`.`fk_status_id` = `character_status`.`status_id` " +
                    //$"WHERE `characters`.`fk_owner_address` = '{address}'";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {

                        characterIds.Add((int)sqlDataReader["id"]);
                    }

                    await conn.CloseAsync();

                    foreach (int characterId in characterIds)
                    {
                        characters.Add(await getCharacter(characterId));
                    }
                    return characters;

                    //await conn.OpenAsync();
                    //MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    //MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    //while (sqlDataReader.Read())
                    //{
                    //    var status = new CharacterStatusViewModel
                    //    {
                    //        Id = (int)sqlDataReader["status_id"],
                    //        Name = (string)sqlDataReader["status_name"],
                    //        Icon = (string)sqlDataReader["icon"],
                    //        IconColor = (string)sqlDataReader["iconColor"],
                    //        BgColor = (string)sqlDataReader["bgColor"],
                    //        Description = (string)sqlDataReader["description"],
                    //        statusDuration = (int)sqlDataReader["status_duration"],
                    //        statusChanged = (long)sqlDataReader["status_changed"],
                    //    };

                    //    characters.Add(new CharacterViewModel
                    //    {
                    //        Id = (int)sqlDataReader["id"],
                    //        Owner = (string)sqlDataReader["fk_owner_address"],
                    //        Name = (string)sqlDataReader["name"],
                    //        Gender = (string)sqlDataReader["gender"],
                    //        Seed = (long)sqlDataReader["seed"],
                    //        Rarity = (string)sqlDataReader["rarity"],
                    //        Power = (int)sqlDataReader["power"],
                    //        Health = (int)sqlDataReader["health"],
                    //        CurrentHealth = (int)sqlDataReader["currentHealth"],
                    //        Stamina = (int)sqlDataReader["stamina"],
                    //        CurrentStamina = (int)sqlDataReader["currentStamina"],
                    //        Job = (string)sqlDataReader["job"],
                    //        Affiliation = (string)sqlDataReader["affiliation"],
                    //        Status = status,
                    //    });
                    //}

                    //await conn.CloseAsync();
                    //return characters;
                }
                else
                {
                    throw new InvalidInputException("address");
                }

            }
            catch (InvalidCastException err)
            {
                throw new InvalidCastException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
