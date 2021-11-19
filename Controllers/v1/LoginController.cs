using back_end.InputModel;
using back_end.Services;
using back_end.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [HttpPost("checksign")]
        public async Task<ActionResult<bool>> checkSignature([FromBody] SignatureInputModel request)
        {
            var login = await _loginService.checkSignature(request);
            if (login)
                return Ok();

            else
                return BadRequest();
        }

        [HttpGet("{address}")]
        public async Task<ActionResult<UserAccountViewModel>> GetAccount([FromRoute] string address)
        {
            var account = await _loginService.getAccount(address);

            if (account == null)
                return NoContent();

            return Ok(account);
        }
    }
}
