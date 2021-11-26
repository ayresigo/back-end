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
        private readonly ITimeService _timeService;

        public CharacterMockController(ICharacterMockService characterMockService, ITimeService timeService)
        {
            _characterMockService = characterMockService;
            _timeService = timeService;
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

        [HttpGet("getCharacters")]
        public async Task<List<CharacterViewModel>> getCharacters([FromQuery] int accountId)
        {
            List<CharacterViewModel> characters = await _characterMockService.getCharacters(accountId);
            return characters;

        }

        [HttpPatch("editCharacterStatus")]
        public async Task editStatus([FromQuery] int characterId, int status = 1, long duration = -1)
        {
            var start = await _timeService.getTime();
            await _characterMockService.editStatus(characterId, status, duration, start);
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
