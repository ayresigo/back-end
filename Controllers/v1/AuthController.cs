using back_end.InputModel;
using back_end.Services;
using back_end.ViewModel;
using JWT.Exceptions;
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

        [HttpPost("generateToken")]
        public async Task<ActionResult<TokenViewModel>> generateToken([FromBody] TokenDataInputModel data)
        {
            var token = await _authService.generateToken(data, _secret);
            return Ok(token);
        }

        [HttpGet("checkToken")]
        public async Task<ActionResult> retrieveToken([FromQuery]string token)
        {
            TokenInputModel _token = new TokenInputModel
            {
                Token = token
            };
         
            try
            {
                var data = await _authService.retrieveToken(_token);
                return Ok(data);
            }
            catch (TokenExpiredException err)
            {
                return Unauthorized(err.Message);
            }
            catch (SignatureVerificationException err)
            {
                return Unauthorized(err.Message);
            }            
        }

        [HttpPost("checkSignature")]
        public async Task<ActionResult<bool>> checkSignature([FromBody]SignatureInputModel request)
        {
            var login = await _authService.checkSignature(request);
            if (login)
                return Ok();

            else
                return BadRequest();
        }
    }
}
