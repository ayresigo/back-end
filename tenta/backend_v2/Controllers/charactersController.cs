using cryminals.Models.ViewModels;
using cryminals.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class charactersController : ControllerBase
    {
        public ICharacterRepository _characterRepo;

        public charactersController(ICharacterRepository characterRepo)
        {
            _characterRepo = characterRepo;
        }

        [HttpGet("getCharacter")]
        public async Task<IActionResult> getCharacter(int id)
        {
            try
            {
                var character = await _characterRepo.getCharacter(id);
                if (character != null)
                {
                    return Ok(character);
                }
                else
                {
                    return NotFound("Not found!");
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("getCharacters")]
        public async Task<IActionResult> getCharacters(string address)
        {
            try
            {
                var characters = await _characterRepo.getCharacters(address);
                if (characters.Count > 0)
                {
                    return Ok(characters);
                }
                else
                {
                    return NotFound("Not found!");
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
