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
    public class accountsController : ControllerBase
    {
        public IAccountRepository _accountRepo;

        public accountsController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpGet("getAccount")]
        public IActionResult getAccount(string address)
        {
            try
            {
                return Ok(_accountRepo.getAccount(address).Result);
            } catch (Exception err)
            {
                return BadRequest(err.Message);
            }
            
        }

        [HttpGet("fetchAccount")]
        public IActionResult fetchAccount(string token)
        {
            try
            {
                return Ok(_accountRepo.fetchAccount(token).Result);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }

        [HttpGet("fetchCharacters")]
        public async Task<ActionResult<IEnumerable<AccountViewModel>>> fetchCharacters(string token)
        {
            try
            {
                var result = await _accountRepo.fetchCharacters(token);
                return Ok(result);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }
    }
}
