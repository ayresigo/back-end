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
    public class utilsController : ControllerBase
    {

        private readonly ICheckInputs _checkInputs;

        public utilsController(ICheckInputs checkInputs)
        {
            _checkInputs = checkInputs;
        }

        [HttpGet("checkInput/address")]
        public IActionResult ChekAddress([FromQuery] string address)
        {
            try
            {
                _checkInputs.checkAddress(address);
                return Ok("Address is valid!");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }
        
        [HttpGet("checkInput/int")]
        public IActionResult CheckInt([FromQuery] int value, int min, int max)
        {
            try
            {
                _checkInputs.checkInt(value, min, max);
                return Ok("Integer is valid!");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }
        
        [HttpGet("checkInput/hexHash")]
        public IActionResult CheckHexHash([FromQuery] string hexHash)
        {
            try
            {
                _checkInputs.checkHexHash(hexHash);
                return Ok("Address is valid!");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }

        [HttpGet("checkInput/token")]
        public IActionResult CheckToken([FromQuery] string token)
        {
            try
            {
                _checkInputs.checkToken(token);
                return Ok("Token is valid!");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("checkInput/base64")]
        public IActionResult CheckBase64([FromQuery] string input)
        {
            try
            {
                _checkInputs.checkBase64(input);
                return Ok("Base64 is valid!");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("time/getUnixTime")]
        public IActionResult getUnixTime()
        {
            return Ok(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
    }
}
