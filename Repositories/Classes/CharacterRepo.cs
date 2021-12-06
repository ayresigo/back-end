using back_end.InputModel;
using back_end.Repositories.Interface;
using back_end.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories.Classes
{
    public class CharacterRepo : ICharacterRepo
    {
        private readonly MySqlConnection conn;
        private readonly IAccountRepo _accountRepo;

        public CharacterRepo(IAccountRepo accountRepo, IConfiguration config)
        {
            _accountRepo = accountRepo;
            conn = new MySqlConnection(config.GetConnectionString("Default"));
        }
        public async Task<bool> addCharacter(CharacterInputModel character, string address)
        {
            var owner = await _accountRepo.getAccount(address);
            var query = $"INSERT INTO `criminals`( `fk_owner_id`, `name`, `gender`, `avatar`, `rarity`, `power`, `moneyRatio`, `health`, `currentHealth`, `stamina`, `currentStamina`, `job`, `alignment`) " +
                $"VALUES ('{owner.Id}','{character.name}','{character.gender}','{character.avatar}','{character.rarity}','{character.power}','{character.moneyRatio}','{character.health}','{character.health}','{character.stamina}','{character.stamina}','{character.job}','{character.alignment}')";

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

        public async Task<CharacterStatusViewModel> getStatus(int id)
        {
            CharacterStatusViewModel status = null;
            var query = $"SELECT * FROM `character_status` WHERE `id` = '{id}'";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                status = new CharacterStatusViewModel
                {
                    id = (int)sqlDataReader["id"],
                    name = (string)sqlDataReader["name"],
                    icon = (string)sqlDataReader["icon"],
                    iconColor = (string)sqlDataReader["icon_color"],
                    bgColor = (string)sqlDataReader["background_color"],
                    description = (string)sqlDataReader["description"],
                };
            }

            await conn.CloseAsync();
            return status;
        }

        public async Task<CharacterViewModel> getCharacter(int id)
        {

            CharacterViewModel character = null;
            var query = $"SELECT * FROM `criminals` " +
                $"INNER JOIN `character_status` " +
                $"ON `criminals`.`fk_status_id` = `character_status`.`status_id` " +
                $"WHERE `criminals`.`id` = {id}";

            await conn.OpenAsync();

            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                var status = new CharacterStatusViewModel
                {
                    id = (int)sqlDataReader["fk_status_id"],
                    name = (string)sqlDataReader["status_name"],
                    icon = (string)sqlDataReader["status_icon"],
                    iconColor = (string)sqlDataReader["status_icon_color"],
                    bgColor = (string)sqlDataReader["status_background_color"],
                    description = (string)sqlDataReader["status_description"],
                };

                character = new CharacterViewModel
                {
                    id = (int)sqlDataReader["id"],
                    owner = (int)sqlDataReader["fk_owner_id"],
                    name = (string)sqlDataReader["name"],
                    gender = (string)sqlDataReader["gender"],
                    avatar = (string)sqlDataReader["avatar"],
                    rarity = (string)sqlDataReader["rarity"],
                    power = (int)sqlDataReader["power"],
                    moneyRatio = (int)sqlDataReader["moneyRatio"],
                    health = (int)sqlDataReader["health"],
                    currentHealth = (int)sqlDataReader["currentHealth"],
                    stamina = (int)sqlDataReader["stamina"],
                    currentStamina = (int)sqlDataReader["currentStamina"],
                    job = (string)sqlDataReader["job"],
                    alignment = (string)sqlDataReader["alignment"],
                    status = status,
                    statusTime = (long)sqlDataReader["statusTime"],
                    statusChanged = (long)sqlDataReader["statusChanged"]
                };
            }

            await conn.CloseAsync();
            return character;
        }

        public async Task<List<CharacterStatusViewModel>> getStatus()
        {
            var query = $"SELECT * FROM `character_status` WHERE 1";
            var status = new List<CharacterStatusViewModel>();

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
  
                status.Add(new CharacterStatusViewModel
                {
                    id = (int)sqlDataReader["status_id"],
                    name = (string)sqlDataReader["status_name"],
                    icon = (string)sqlDataReader["status_icon"],
                    iconColor = (string)sqlDataReader["status_icon_color"],
                    bgColor = (string)sqlDataReader["status_background_color"],
                    description = (string)sqlDataReader["status_description"],
                });
            }

            await conn.CloseAsync();
            return status;
        }

        public async Task<List<CharacterViewModel>> fetchCharacterStatus(int id)
        {
            var characters = new List<CharacterViewModel>();
            characters = await getCharacters(id);
            var status = await getStatus();

            foreach (var character in characters)
            {
                if (character.status.id != 1)
                {
                    var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                    if (currentTime - character.statusChanged >= character.statusTime)
                    {
                        character.status = status[0];
                        var query = $"UPDATE `criminals` SET `fk_status_id`='{character.status.id}',`statusTime`='{0}',`statusChanged`='{currentTime}' WHERE id='{character.id}'";

                        await conn.OpenAsync();
                        MySqlCommand sqlCommand = new MySqlCommand(query, conn);
                        MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                        await conn.CloseAsync();                        
                    }
                }
            }
            return characters;
        }

        public async Task<List<CharacterViewModel>> getCharacters(int id)
        {
            var characters = new List<CharacterViewModel>();
            var query = $"SELECT * FROM `criminals` " +
                $"INNER JOIN `character_status` " +
                $"ON `criminals`.`fk_status_id` = `character_status`.`status_id` " +
                $"WHERE `criminals`.`fk_owner_id` = {id}";

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                var status = new CharacterStatusViewModel
                {
                    id = (int)sqlDataReader["fk_status_id"],
                    name = (string)sqlDataReader["status_name"],
                    icon = (string)sqlDataReader["status_icon"],
                    iconColor = (string)sqlDataReader["status_icon_color"],
                    bgColor = (string)sqlDataReader["status_background_color"],
                    description = (string)sqlDataReader["status_description"],
                };

                characters.Add(new CharacterViewModel
                {
                    id = (int)sqlDataReader["id"],
                    owner = (int)sqlDataReader["fk_owner_id"],
                    name = (string)sqlDataReader["name"],
                    gender = (string)sqlDataReader["gender"],
                    avatar = (string)sqlDataReader["avatar"],
                    rarity = (string)sqlDataReader["rarity"],
                    power = (int)sqlDataReader["power"],
                    moneyRatio = (int)sqlDataReader["moneyRatio"],
                    health = (int)sqlDataReader["health"],
                    currentHealth = (int)sqlDataReader["currentHealth"],
                    stamina = (int)sqlDataReader["stamina"],
                    currentStamina = (int)sqlDataReader["currentStamina"],
                    job = (string)sqlDataReader["job"],
                    alignment = (string)sqlDataReader["alignment"],
                    status = status,
                    statusTime = (long)sqlDataReader["statusTime"],
                    statusChanged = (long)sqlDataReader["statusChanged"],
                });
            }

            await conn.CloseAsync();
            return characters;
        }

        public async Task editHealth(int id, int amount)
        {
            var query = $"UPDATE `criminals` SET `currentHealth`= currentHealth + {amount} WHERE id='{id}'";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }
        public async Task editStamina(int id, int amount)
        {
            var query = $"UPDATE `criminals` SET `currentStamina`= currentStamina + {amount} WHERE id='{id}'";
            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }

        public async Task editStatus(int id, int status = 1, long duration = -1, long start = 0)
        {
            long time = 0;
            if (start == 0)
                time = DateTimeOffset.Now.ToUnixTimeSeconds();


            var query = $"UPDATE `criminals` SET `fk_status_id`='{status}',`statusTime`='{duration}',`statusChanged`='{time}' WHERE id='{id}'";

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            await conn.CloseAsync();
        }
    }
}
