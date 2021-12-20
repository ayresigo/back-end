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
    public class RobberyEventRepository : IRobberyEventRepository
    {
        private readonly MySqlConnection conn;
        private readonly ICheckInputs _checkInputs;
        private readonly IAuthService _authService;
        private readonly IAccountRepository _accountRepository;
        private readonly ICharacterRepository _characterRepository;
        //private readonly IRobberyRepository _robberyRepository;

        public RobberyEventRepository(IConfiguration config, ICheckInputs checkInputs, IAuthService authService, ICharacterRepository characterRepository)//, IRobberyRepository robberyRepository)
        {
            conn = new MySqlConnection(config.GetConnectionString("Default"));
            _checkInputs = checkInputs;
            _authService = authService;
            _characterRepository = characterRepository;
            //_robberyRepository = robberyRepository;
        }

        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task startRobbery(CharacterViewModel character, RobberyViewModel robbery)
        {
            await _characterRepository.editCurrentStat(character.Id, "stamina", character.CurrentStamina - robbery.Stamina); // aplica - stamina e inicia a robbery
            await _characterRepository.changeStatus(character.Id, 2, robbery.Duration); // edita o status do personagem para trabalhando
            var query = $"INSERT INTO `robbery_events`(`fk_robbery_id`,`fk_character_id`,`start`,`duration`) VALUES ({robbery.Id},{character.Id},{DateTimeOffset.Now.ToUnixTimeSeconds()},{robbery.Duration})";

            await conn.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(query, conn);
            sqlCommand.ExecuteNonQuery();
            await conn.CloseAsync();
        }
    }
}
