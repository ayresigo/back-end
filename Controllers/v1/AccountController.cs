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
using back_end.Services.Interfaces;

namespace back_end.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICheckInputs _checkInputs;
        public AccountController(IAccountService accountService, ICheckInputs checkInputs)
        {
            _accountService = accountService;
            _checkInputs = checkInputs;
        }

        //[HttpGet("checkAddress")]
        //public bool checkAddress(string address)
        //{
        //    Regex rx = new Regex("^0x[a-fA-F0-9]{40}$");
        //    if (rx.Match(address).Success)
        //        return true;
        //    else
        //        return false;
        //}

        //[HttpGet("checkBase64Input")]
        //public bool checkBase64Input(string input)
        //{
        //    Regex rx = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$"); //base64
        //    if (rx.Match(input).Success)
        //        return true;
        //    else
        //        return false;
        //}

        [HttpGet("getAccount")]
        public async Task<ActionResult<AccountViewModel>> getAccount([FromQuery] string address)
        {
            if ( _checkInputs.checkAddress(address))
            {
                var account = await _accountService.getAccount(address);
                if (account.username != "null")
                {
                    byte[] data = Convert.FromBase64String(account.username);
                    string realUsername = Encoding.UTF8.GetString(data);
                    account.username = realUsername;
                }                

                if (account == null) return NoContent(); else return Ok(account);
            }
            else return BadRequest();
        }

        [HttpPatch("editUsername")]
        public async Task<ActionResult> editUsername([FromQuery] string address, [FromQuery] string username)
        {
            if (_checkInputs.checkAddress(address) && _checkInputs.checkBase64Input(username))
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
            if (_checkInputs.checkAddress(address))
            {
                if (await _accountService.createAccount(address))
                    return Ok();
            }
            return BadRequest();
        }
    }
}
