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
    public interface ICharacterRepository : IDisposable
    {
        public Task<CharacterViewModel> getCharacter(int id);
        public Task<List<CharacterViewModel>> fetchCharacters(string token, int page = 1, int itemsPerPage = 9);
        public Task<List<CharacterViewModel>> getCharacters(string address);
        public Task<string> getOwner(int id);
        public Task editCurrentStat(int characterId, string stat, int amount);
        public Task changeStatus(int characterId, int status, int duration);
        public Task<CharacterStatusViewModel> getCharacterStatus(int id);
        public int calculateCurrentStamina(CharacterViewModel character);
        public Task<long> getCharacterQtd(string address);
    }

    public class CharacterToUpdate
    {
        public int Id { get; set; }
        public int CurrentStamina { get; set; }
    }

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

        public async Task<long> getCharacterQtd(string address)
        {
            if (_checkInputs.checkAddress(address))
            {

                var query = $"SELECT COUNT(*) FROM characters WHERE fk_owner_address = '{address}'";
                long qtd = 0;

                await conn.OpenAsync();
                MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                while (sqlDataReader.Read())
                {
                    qtd = (long)sqlDataReader[0];
                }
                await conn.CloseAsync();
                return qtd;
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

                    var query = $"UPDATE `characters` SET `fk_status_id`={status}, `status_duration`={duration}, `status_changed`={DateTimeOffset.UtcNow.ToUnixTimeSeconds()} WHERE `id`={characterId}";

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

        public async Task<CharacterStatusViewModel> getCharacterStatus(int id)
        {
            try
            {
                if (_checkInputs.checkInt(id))
                {
                    var query = $"SELECT * FROM `character_status` " +
                        $"WHERE `character_status`.`status_id` = {id}";
                    CharacterStatusViewModel status = null;

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        status = new CharacterStatusViewModel
                        {
                            Id = (int)sqlDataReader["status_id"],
                            Name = (string)sqlDataReader["status_name"],
                            Icon = (string)sqlDataReader["icon"],
                            IconColor = (string)sqlDataReader["iconColor"],
                            BgColor = (string)sqlDataReader["bgColor"],
                            Description = (string)sqlDataReader["description"],
                            statusDuration = 0,
                            statusChanged = 0,
                        };
                    }

                    await conn.CloseAsync();

                    return status;
                }
                else throw new InvalidInputException("id");
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public int calculateCurrentStamina(CharacterViewModel character)
        {
            if (character.Status.Id == 1 && character.CurrentStamina < character.Stamina)
            {
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var regenTime = 3600;

                var regeneratedStamina = character.CurrentStamina + Convert.ToInt32((currentTime - (character.Status.statusChanged + character.Status.statusDuration)) / (regenTime / (character.StaminaRatio / 100)));
                if (regeneratedStamina >= character.Stamina) return character.Stamina;
                else return regeneratedStamina;
            }
            else
            {
                return character.CurrentStamina;
            }
        }

        public async Task<List<CharacterViewModel>> fetchCharacters(string token, int page = 1, int itemsPerPage = 9)
        {
            try
            {
                if (_checkInputs.checkToken(token) && _checkInputs.checkInt(page, 1) && _checkInputs.checkInt(itemsPerPage, 1))
                {
                    var account = _authService.retrieveTokenData(token);
                    var characters = new List<CharacterViewModel>();
                    var charactersToUpdate = new List<int>();
                    var query = $"SELECT * FROM `characters` INNER JOIN `character_status`  " +
                        $"ON `characters`.`fk_status_id` = `character_status`.`status_id` " +
                        $"WHERE `characters`.`fk_owner_address` = '{account.address}' LIMIT {itemsPerPage} OFFSET {(page - 1) * 9}";
                    var idleStatus = await getCharacterStatus(1);
                    var status = new CharacterStatusViewModel();
                    var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        if ((int)sqlDataReader["status_duration"] + (long)sqlDataReader["status_changed"] <= currentTime && (int)sqlDataReader["status_id"] != 1)
                        {
                            status = new CharacterStatusViewModel
                            {
                                Id = idleStatus.Id,
                                Name = idleStatus.Name,
                                Icon = idleStatus.Icon,
                                IconColor = idleStatus.IconColor,
                                BgColor = idleStatus.BgColor,
                                Description = idleStatus.Description,
                                statusDuration = 1,
                                statusChanged = currentTime,
                            };
                            charactersToUpdate.Add((int)sqlDataReader["id"]);
                        }

                        else
                        {
                            status = new CharacterStatusViewModel
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
                        }

                        var character = new CharacterViewModel
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
                            StaminaRatio = (int)sqlDataReader["staminaRatio"],
                            Job = (string)sqlDataReader["job"],
                            Affiliation = (string)sqlDataReader["affiliation"],
                            Status = status,
                        };

                        character.CurrentStamina = calculateCurrentStamina(character);
                        characters.Add(character);
                    }

                    await conn.CloseAsync();

                    foreach (int id in charactersToUpdate)
                    {
                        await changeStatus(id, 1, 1);
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
        }

        public async Task<CharacterViewModel> getCharacter(int id)
        {
            try
            {
                if (_checkInputs.checkInt(id))
                {
                    CharacterStatusViewModel status = null;
                    CharacterViewModel character = null;
                    var idleStatus = await getCharacterStatus(1);
                    var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var updateCharacter = false;
                    var query = $"SELECT * FROM `characters` INNER JOIN `character_status`  " +
                    $"ON `characters`.`fk_status_id` = `character_status`.`status_id` " +
                    $"WHERE `characters`.`id` = '{id}'";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {

                        if ((int)sqlDataReader["status_duration"] + (long)sqlDataReader["status_changed"] <= currentTime && (int)sqlDataReader["status_id"] != 1)
                        {
                            status = new CharacterStatusViewModel
                            {
                                Id = idleStatus.Id,
                                Name = idleStatus.Name,
                                Icon = idleStatus.Icon,
                                IconColor = idleStatus.IconColor,
                                BgColor = idleStatus.BgColor,
                                Description = idleStatus.Description,
                                statusDuration = 1,
                                statusChanged = currentTime,
                            };
                            updateCharacter = true;
                        }

                        else
                        {
                            status = new CharacterStatusViewModel
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
                        }


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
                            StaminaRatio = (int)sqlDataReader["staminaRatio"],
                            Job = (string)sqlDataReader["job"],
                            Affiliation = (string)sqlDataReader["affiliation"],
                            Status = status,
                        };

                        character.CurrentStamina = calculateCurrentStamina(character);
                    }

                    await conn.CloseAsync();

                    if (updateCharacter)
                    {
                        await changeStatus(character.Id, 1, 1);
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
