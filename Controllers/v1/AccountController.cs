using back_end.InputModel;
using back_end.Repositories;
using back_end.Services;
using back_end.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text;

namespace back_end.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("checkAddress")]
        public bool checkAddress(string address)
        {
            Regex rx = new Regex("^0x[a-fA-F0-9]{40}$");
            if (rx.Match(address).Success)
                return true;
            else
                return false;
        }

        [HttpGet("checkBase64Input")]
        public bool checkBase64Input(string input)
        {
            Regex rx = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$"); //base64
            if (rx.Match(input).Success)
                return true;
            else
                return false;
        }

        [HttpGet("getAccount")]
        public async Task<ActionResult<AccountViewModel>> getAccount([FromQuery] string address)
        {
            if (checkAddress(address))
            {
                var account = await _accountService.getAccount(address);
                if (account == null) return NoContent(); else return Ok(account);
            }
            else return BadRequest();
        }

        [HttpPatch("editUsername")]
        public async Task<ActionResult> editUsername([FromQuery] string address, [FromQuery] string username)
        {
            if (checkAddress(address) && checkBase64Input(username))
            {
                if (await _accountService.editUsername(address, username))
                    return Ok();
                else
                    return NoContent();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("createAccount")]
        public async Task<ActionResult> createAccount([FromQuery] string address)
        {
            if (checkAddress(address))
            {
                if (await _accountService.createAccount(address))
                    return Ok();
            }
            return BadRequest();
        }
    }
}
