using back_end.InputModel;
using back_end.Services;
using back_end.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        const string _secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("tokenGen")]
        public async Task<ActionResult<TokenViewModel>> generateToken([FromBody] TokenDataInputModel data, string secret = _secret)
        {
            var token = await _authService.generateToken(data, secret);
            return Ok(token);
        }

        [HttpPost("retrieveToken")]
        public async Task<ActionResult> retrieveToken([FromBody] TokenInputModel token, string secret = _secret)
        {
            var data = await _authService.retrieveToken(token);
            return Ok(data);
        }
    }
}
