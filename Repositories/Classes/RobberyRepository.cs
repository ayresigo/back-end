using cryminals.Exceptions;
using cryminals.Models.InputModels;
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
    public interface IRobberyRepository : IDisposable
    {
        public Task<List<RobberyViewModel>> getRobberies(int status);
        public Task<RobberyViewModel> getRobbery(int id);
        public Task<string> startRobbery(StartRobberyInputModel data);
        public Task<List<RobberyEventViewModel>> getRobberyEvent(int characterId, int claimed, int status);
        public Task endRobberyEvent(int robberyEventId, int status);
    }
    public class RobberyRepository : IRobberyRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;
        private readonly IAuthService _authService;
        private readonly ICharacterRepository _characterRepository;
        private readonly IRobberyEventRepository _robberyEventRepository;


        public RobberyRepository(IConfiguration config, ICheckInputs checkInputs, IAuthService authService, ICharacterRepository characterRepository, IRobberyEventRepository robberyEventRepository)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _checkInputs = checkInputs;
            _authService = authService;
            _characterRepository = characterRepository;
            _robberyEventRepository = robberyEventRepository;
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task endRobberyEvent(int robberyEventId, int status)
        {
            try
            {
                if (_checkInputs.checkInt(robberyEventId) && _checkInputs.checkInt(status, 1, 3))
                {
                    var query = $"UPDATE `robbery_events` SET `status`={status} WHERE `id`={robberyEventId}";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                    await conn.CloseAsync();
                }
                else throw new InvalidInputException("input(s)");
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<List<RobberyEventViewModel>> getRobberyEvent(int characterId, int claimed, int status)
        {
            try
            {
                if (_checkInputs.checkInt(characterId) && _checkInputs.checkInt(claimed, 0, 2) && _checkInputs.checkInt(status, 1, 4))
                {
                    List<RobberyEventViewModel> robberyEvent = new List<RobberyEventViewModel>();

                    var claimedString = "";
                    if (claimed != 2)
                    {
                        // 0 - unclaimed
                        // 1 - claimed
                        // 2 - all
                        if (claimed == 1) claimedString = $"AND `claimed` != {0}";
                        else claimedString = $"AND `claimed` = {0}";
                    }

                    var statusString = "";
                    if (status != 4)
                    {
                        // 1 - in progress
                        // 2 - succeed
                        // 3 - failed
                        statusString = $"AND `status` = {status}";
                    }
                    var query = $"SELECT * FROM `robbery_events` WHERE `fk_character_id` = {characterId} {claimedString} {statusString}";


                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        robberyEvent.Add(new RobberyEventViewModel
                        {
                            Id = (int)sqlDataReader["Id"],
                            RobberyId = (int)sqlDataReader["fk_robbery_id"],
                            CharacterId = (int)sqlDataReader["fk_character_id"],
                            Started = (long)sqlDataReader["start"],
                            Duration = (int)sqlDataReader["duration"],
                            Status = (string)sqlDataReader["status"],
                            Claimed = (long)sqlDataReader["claimed"],
                        });
                    }

                    await conn.CloseAsync();
                    return robberyEvent;
                }
                else throw new InvalidInputException("input(s)");
            }
            catch (Exception err) { throw new Exception(err.Message); }
        }

        public async Task<string> startRobbery(StartRobberyInputModel data)
        {
            // Verificar robbery, status do personagem e calcular as odds.
            try
            {
                if (_checkInputs.checkToken(data.Token) && _checkInputs.checkInt(data.RobberyId)) // valida input do token e do id da robbery
                {
                    for (int i = 0; i < data.Participants.Length; i++)
                    {
                        if (!_checkInputs.checkInt(data.Participants[i])) throw new InvalidInputException("Invalid Input"); // valida input dos ids dos participantes
                        for (int j = 0; j < data.Participants.Length; j++)
                        {
                            if (data.Participants[j] == data.Participants[i] && j != i) throw new Exception("Duplicated characters");
                        }
                    }
                    var account = _authService.retrieveTokenData(data.Token);
                    var robbery = await getRobbery(data.RobberyId);
                    if (data.Participants.Length >= robbery.MinParticipants && data.Participants.Length <= robbery.MaxParticipants) // valida se o numero de participantes está dentro dos limites da robbery
                    {
                        if (await _authService.checkOwnership(account.address, data.Participants)) // verifica se os personagens são possuídos pelo caller (deveria ser feito pela blockchain)
                        {
                            List<CharacterViewModel> characters = new List<CharacterViewModel>();
                            for (int i = 0; i < data.Participants.Length; i++)
                            {
                                var character = await _characterRepository.getCharacter(data.Participants[i]);
                                if (_characterRepository.calculateCurrentStamina(character) >= robbery.Stamina && character.Status.Id == 1) characters.Add(character); else throw new Exception("Character " + character.Id + " do not have enough stamina or is busy"); // verifica se cada personagem possui stamina suficiente e status idle
                            }
                            if (characters.Count == data.Participants.Length)
                            {
                                foreach (CharacterViewModel character in characters)
                                {
                                    await _robberyEventRepository.startRobbery(character, robbery);
                                }
                                return "Roubo iniciado!";
                            }
                            else throw new Exception("One or more characters are not elegible to start the robbery");
                        }
                        else throw new Exception("Character ownership verification failed");
                    }
                    else throw new Exception("Number of participants is not permitted");
                }
                else throw new InvalidInputException("Invalid input");
                return "Roubo não iniciado!";
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public async Task<List<RobberyViewModel>> getRobberies(int status = 1)
        {
            try
            {
                if (_checkInputs.checkInt(status))
                {
                    var robberies = new List<RobberyViewModel>();
                    var query = $"SELECT * FROM `robberies` WHERE `status` = {status}";

                    await conn.OpenAsync();
                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        robberies.Add(new RobberyViewModel
                        {
                            Id = (int)sqlDataReader["Id"],
                            Name = (string)sqlDataReader["Name"],
                            Description = (string)sqlDataReader["Description"],
                            Reward = (int)sqlDataReader["Reward"],
                            Duration = (int)sqlDataReader["Duration"],
                            Stamina = (int)sqlDataReader["Stamina"],
                            Power = (int)sqlDataReader["Power"],
                            MinParticipants = (int)sqlDataReader["MinParticipants"],
                            MaxParticipants = (int)sqlDataReader["MaxParticipants"],
                            AmbushRisk = (int)sqlDataReader["AmbushRisk"],
                            PrisonRisk = (int)sqlDataReader["PrisonRisk"],
                            DeathRisk = (int)sqlDataReader["DeathRisk"],
                            Background = (string)sqlDataReader["Bg"]
                        });
                    }

                    await conn.CloseAsync();
                    return robberies;
                }
                else
                {
                    throw new InvalidInputException("status");
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

        public async Task<RobberyViewModel> getRobbery(int id)
        {
            try
            {
                if (_checkInputs.checkInt(id))
                {
                    RobberyViewModel robbery = null;
                    var query = $"SELECT * FROM `robberies` WHERE `id` = '{id}'";

                    await conn.OpenAsync();

                    MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

                    while (sqlDataReader.Read())
                    {
                        robbery = new RobberyViewModel
                        {
                            Id = (int)sqlDataReader["Id"],
                            Name = (string)sqlDataReader["Name"],
                            Description = (string)sqlDataReader["Description"],
                            Reward = (int)sqlDataReader["Reward"],
                            Duration = (int)sqlDataReader["Duration"],
                            Stamina = (int)sqlDataReader["Stamina"],
                            Power = (int)sqlDataReader["Power"],
                            MinParticipants = (int)sqlDataReader["MinParticipants"],
                            MaxParticipants = (int)sqlDataReader["MaxParticipants"],
                            AmbushRisk = (int)sqlDataReader["AmbushRisk"],
                            PrisonRisk = (int)sqlDataReader["PrisonRisk"],
                            DeathRisk = (int)sqlDataReader["DeathRisk"],
                            Background = (string)sqlDataReader["Bg"]
                        };
                    }

                    await conn.CloseAsync();
                    return robbery;
                }
                else
                {
                    throw new InvalidInputException("id");
                }
            }
            catch (InvalidInputException ex)
            {
                throw new InvalidInputException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
