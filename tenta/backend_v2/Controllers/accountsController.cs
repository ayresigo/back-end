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
                return Ok(_accountRepo.getAccount(address));
            } catch (Exception err)
            {
                return BadRequest(err.Message);
            }
            
        }

        [HttpGet("getAccount")]
        public IActionResult getMyAccount(string token)
        {
            try
            {
                return Ok(_accountRepo.getMyAccount(token));
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }
    }
}
