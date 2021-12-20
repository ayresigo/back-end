using cryminals.Models.InputModels;
using cryminals.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        public IAuthService _authService;

        public authController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("retrieveTokenData")]
        public IActionResult retrieveTokenData(string token)
        {
            try
            {
                return Ok(_authService.retrieveTokenData(token));
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost("generateToken")]
        public IActionResult generateToken([FromBody] SignatureInputModel data)
        {
            try
            {
                return Ok(_authService.generateToken(data));
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost("validateSignature")]
        public IActionResult validateSignature([FromBody] SignatureInputModel signature)
        {
            try
            {
                return Ok(_authService.validateSignature(signature));
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost("checkOwnership")]
        public async Task<IActionResult> checkOwnership(string address, int[] ids)
        {
            try
            {
                if (await _authService.checkOwnership(address, ids))
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }
    }
}
