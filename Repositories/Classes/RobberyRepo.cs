using back_end.InputModel;
using back_end.Repositories.Interface;
using back_end.Services;
using back_end.Services.Interfaces;
using back_end.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories.Classes
{
    public class RobberyRepo : IRobberyRepo
    {
        private readonly MySqlConnection conn;
        private readonly ICharacterMockService _characterService;
        private readonly IAuthService _authService;

        public RobberyRepo(IConfiguration config, ICharacterMockService characterService, IAuthService authService)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _characterService = characterService;
            _authService = authService;
        }

        public async Task addLogs(RobberyLogViewModel log)
        {
            var query = $"INSERT INTO `robberies_logs`( `robbery_id`, `fk_sender_id`, `fk_character_id`, `fk_robbery_id`, `participants`, `start_money`, `start_stamina`, " +
                $"`start_health`, `start_respect`, `start_date`, `end_date`, `end_health`, `end_money`, `robbery_status`, `server_status`) " +
                $"VALUES ('{1}','{log.senderId}','{log.characterId}','{log.robberyId}','{log.participants}','{log.startMoney}','{log.startStamina}'," +
                $"'{log.startHealth}', '{log.startRespect}','{log.startDate}','{log.endDate}','{log.endHealth}','{log.endMoney}','{log.robberyStatus}','{log.serverStatus}')";

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            sqlCommand.ExecuteNonQuery();
            await conn.CloseAsync();
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<List<RobberyViewModel>> getRobberies(int status)
        {
            var robberies = new List<RobberyViewModel>();
            var query = $"SELECT * FROM `robberies` WHERE `status` = '{status}'";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                robberies.Add(new RobberyViewModel
                {
                    id = (int)sqlDataReader["id"],
                    name = (string)sqlDataReader["name"],
                    image = (string)sqlDataReader["image"],
                    description = (string)sqlDataReader["description"],
                    difficulty = (int)sqlDataReader["difficulty"],
                    time = (int)sqlDataReader["duration"],
                    power = (int)sqlDataReader["power"],
                    reward = (int)sqlDataReader["reward"],
                    stamina = (int)sqlDataReader["stamina"],
                    minPart = (int)sqlDataReader["minPart"],
                    maxPart = (int)sqlDataReader["maxPart"],
                    ambush = (int)sqlDataReader["ambush_risk"],
                    prison = (int)sqlDataReader["prison_risk"],
                    death = (int)sqlDataReader["death_risk"],
                    status = (int)sqlDataReader["status"]
                });
            }

            await conn.CloseAsync();
            return robberies;
        }

        public async Task<RobberyViewModel> getRobbery(int id)
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
                    id = (int)sqlDataReader["id"],
                    name = (string)sqlDataReader["name"],
                    image = (string)sqlDataReader["image"],
                    description = (string)sqlDataReader["description"],
                    difficulty = (int)sqlDataReader["difficulty"],
                    time = (int)sqlDataReader["duration"],
                    power = (int)sqlDataReader["power"],
                    reward = (int)sqlDataReader["reward"],
                    stamina = (int)sqlDataReader["stamina"],
                    minPart = (int)sqlDataReader["minPart"],
                    maxPart = (int)sqlDataReader["maxPart"],
                    ambush = (int)sqlDataReader["ambush_risk"],
                    prison = (int)sqlDataReader["prison_risk"],
                    death = (int)sqlDataReader["death_risk"],
                    status = (int)sqlDataReader["status"]
                };
            }

            await conn.CloseAsync();
            return robbery;
        }

        public async Task<List<RobberyLogViewModel>> getCharacterRobberyLogs(int characterId)
        {
            var robberyLog = new List<RobberyLogViewModel>();
            var query = $"SELECT * FROM `robberies_logs` WHERE `fk_character_id` = '{characterId}'";

            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                robberyLog.Add(new RobberyLogViewModel
                {
                    robberyUniqueId = (int)sqlDataReader["id"],
                    senderId = (int)sqlDataReader["fk_sender_id"],
                    characterId = (int)sqlDataReader["fk_character_id"],
                    robberyId = (int)sqlDataReader["fk_robbery_id"],
                    participants = (int)sqlDataReader["participants"],
                    startMoney = (int)sqlDataReader["start_money"],
                    startStamina = (int)sqlDataReader["start_stamina"],
                    startHealth = (int)sqlDataReader["start_health"],
                    startRespect = (int)sqlDataReader["start_respect"],
                    startDate = (int)sqlDataReader["start_date"],
                    endDate = (int)sqlDataReader["end_date"],
                    endHealth = (int)sqlDataReader["end_health"],
                    endMoney = (int)sqlDataReader["end_money"],
                    robberyStatus = (string)sqlDataReader["robbery_status"],
                    serverStatus = (int)sqlDataReader["server_status"],
                });
            }

            await conn.CloseAsync();
            return robberyLog;
        }

        public async Task<RobberyLogViewModel> getCurrentRobbery(int characterId)
        {
            RobberyLogViewModel robberyLog = null;
            var query = $"SELECT * FROM `robberies_logs` WHERE `robbery_status` = '{1}'";

            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                var status = await _characterService.getStatus((int)sqlDataReader["fk_char_end_status_id"]);
                robberyLog = new RobberyLogViewModel
                {
                    robberyUniqueId = (int)sqlDataReader["id"],
                    senderId = (int)sqlDataReader["fk_sender_id"],
                    characterId = (int)sqlDataReader["fk_character_id"],
                    robberyId = (int)sqlDataReader["fk_robbery_id"],
                    participants = (int)sqlDataReader["participants"],
                    startMoney = (int)sqlDataReader["start_money"],
                    startStamina = (int)sqlDataReader["start_stamina"],
                    startHealth = (int)sqlDataReader["start_health"],
                    startRespect = (int)sqlDataReader["start_respect"],
                    startDate = (int)sqlDataReader["start_date"],
                    endDate = (int)sqlDataReader["end_date"],
                    endHealth = (int)sqlDataReader["end_health"],
                    endMoney = (int)sqlDataReader["end_money"],
                    robberyStatus = (string)sqlDataReader["robbery_status"],
                    serverStatus = (int)sqlDataReader["server_status"],
                    charStatus = status,
                    charStatusDuration = (long)sqlDataReader["char_status_duration"]
                };
            }

            await conn.CloseAsync();
            return robberyLog;
        }

        public async Task fetchRobberyResult(int characterId, int robberyId)
        {
            var currentCharacterRobbery = await getCurrentRobbery(characterId);
            var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (currentCharacterRobbery.endDate <= currentTime)
            {
                var character = await _characterService.getCharacter(characterId);
                character.currentHealth = currentCharacterRobbery.endHealth;
                character.currentStamina = currentCharacterRobbery.endStamina;
                character.status = currentCharacterRobbery.charStatus;
                character.statusTime = currentCharacterRobbery.charStatusDuration;
                character.statusChanged = currentTime;
            }
        }

        public async Task startRobbery(StartRobberyInputModel input)
        {
            var tokenInfo = await _authService.retrieveToken(input.token);
            RobberyViewModel robbery = await getRobbery(input.robberyId);
            var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            // check token
            // check if participants belong to owner

            if (input.participants.Length >= robbery.minPart && input.participants.Length <= robbery.maxPart)
            {

                //check if have any item
                //private int prison = new Random();
                
                // throw new robbery
                foreach (var character in input.participants)
                {
                    await _characterService.editStatus(character.characterId, 2, 120);
                    await _characterService.editStamina(character.characterId, -robbery.stamina);
                        
                }
            } else
            {
                throw new Exception("Quantidade de participantes excede limite minimo ou máximo ("+input.participants.Length+")");
            }
        }
    }
}
