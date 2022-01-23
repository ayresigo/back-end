using cryminals.Models.ViewModels;
using cryminals.Repositories.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class accountsController : ControllerBase
    {
        public IAccountRepository _accountRepo;
        public ICharacterRepository _characterRepo;
        public IItemRepository _itemRepository;

        public accountsController(IAccountRepository accountRepo, ICharacterRepository characterRepository, IItemRepository itemRepository)
        {
            _accountRepo = accountRepo;
            _characterRepo = characterRepository;
            _itemRepository = itemRepository;
        }

        [HttpGet("getAccount")]
        public async Task<IActionResult> getAccount(string address)
        {
            try
            {
                var account = await _accountRepo.getAccount(address);
                if (account != null)
                {
                    return Ok(account);
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

        [HttpGet("fetchAccount")]
        public async Task<IActionResult> fetchAccount(string token)
        {
            try
            {
                var account = await _accountRepo.fetchAccount(token);
                if (account != null)
                {
                    return Ok(account);
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


        [HttpGet("fetchItems")]
        public async Task<IActionResult> fetchItems(string token)
        {
            try
            {
                var result = await _itemRepository.fetchItems(token);
                if (result != null)
                {
                    return Ok(result);
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
        [HttpGet("fetchCharacters")]
        //<IEnumerable<CharacterViewModel>>
        public async Task<IActionResult> fetchCharacters(string token, int page = 1, int itemsPerPage = 9)
        {
            try
            {
                var result = await _characterRepo.fetchCharacters(token, page, itemsPerPage);
                if (result != null)
                {
                    return Ok(result);
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
