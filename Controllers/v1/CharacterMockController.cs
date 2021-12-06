using back_end.Entities;
using back_end.InputModel;
using back_end.Repositories;
using back_end.Repositories.Interface;
using back_end.Services;
using back_end.Services.Interfaces;
using back_end.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CharacterMockController : ControllerBase
    {
        private readonly ICharacterMockService _characterMockService;
        private readonly IRobberyService _robberyService;
        private readonly ITimeService _timeService;

        public CharacterMockController(ICharacterMockService characterMockService, ITimeService timeService, IRobberyService robberyService)
        {
            _characterMockService = characterMockService;
            _timeService = timeService;
            _robberyService = robberyService;
        }

        [HttpPost("addCharacter")]
        public async Task<ActionResult> addCharacter([FromBody] CharacterInputModel character, string address)
        {
            CharacterInputModel _character = new CharacterInputModel
            {
                name = character.name,
                gender = character.gender,
                avatar = character.avatar,
                rarity = character.rarity,
                power = character.power,
                moneyRatio = character.moneyRatio,
                health = character.health,
                stamina = character.stamina,
                job = character.job,
                alignment = character.alignment
            };
            await _characterMockService.addCharacter(_character, address);
            return Ok();
        }

        [HttpGet("getCharacter")]
        public async Task<CharacterViewModel> getCharacter([FromQuery] int characterId)
        {
            return await _characterMockService.getCharacter(characterId);
        }
        [HttpGet("getStatus")]
        public async Task<CharacterStatusViewModel> getStatus([FromQuery] int statusId)
        {
            var status = await _characterMockService.getStatus(statusId);
            return status;
        }

        [HttpGet("getCharacters")]
        public async Task<List<CharacterViewModel>> getCharacters([FromQuery] int accountId)
        {
            List<CharacterViewModel> characters = await _characterMockService.getCharacters(accountId);
            foreach (var character in characters)
            {

            }
            return characters;

        }

        [HttpPatch("setHealth")]
        public async Task editHealth([FromQuery] int characterId, int amount)
        {
            await _characterMockService.editHealth(characterId, amount);
        }

        [HttpPatch("setStamina")]
        public async Task editStamina([FromQuery] int characterId, int amount)
        {
            await _characterMockService.editStamina(characterId, amount);
        }

        [HttpPatch("setStatus")]
        public async Task editStatus([FromQuery] int characterId, int status = 1, long duration = -1)
        {
            var start = await _timeService.getTime();
            await _characterMockService.editStatus(characterId, status, duration, start);
        }

        [HttpPatch("fetchCharacters")]
        public async Task<List<CharacterViewModel>> fetchCharacterStatus(int id)
        {
            var characters = await _characterMockService.fetchCharacterStatus(id);
            return characters;
        }

        [HttpGet("createCharacter")]
        public async Task<Character> createCharacter()
        {
            var character = await _characterMockService.createCharacter();

            return new Character
            {
                name = character.name,
                gender = character.gender,
                avatar = character.avatar,
                rarity = character.rarity,
                power = character.power,
                moneyRatio = character.moneyRatio,
                health = character.health,
                stamina = character.stamina,
                job = character.job,
                alignment = character.alignment
            };
        }
    }
}
