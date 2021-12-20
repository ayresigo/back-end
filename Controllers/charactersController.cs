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
        
        [HttpGet("getOwner")]
        public async Task<IActionResult> getOwner(int id)
        {
            try
            {
                var address = await _characterRepo.getOwner(id);
                if (address != null)
                {
                    return Ok(address);
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

        [HttpPatch("editCurrentStamina")]
        public async Task<IActionResult> editCurrentStamina(int id, int amount)
        {
            try
            {
                await _characterRepo.editCurrentStat(id, "stamina", amount);
                return Ok("Stamina applied");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        } 
        
        [HttpPatch("editCurrentHealth")]
        public async Task<IActionResult> editCurrentHealth(int id, int amount)
        {
            try
            {
                await _characterRepo.editCurrentStat(id, "health", amount);
                return Ok("Health applied");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        } 
        
        [HttpPatch("editStatus")]
        public async Task<IActionResult> editStatus(int id, int status, int duration)
        {
            try
            {
                await _characterRepo.changeStatus(id, status, duration);
                return Ok("Status applied");
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
