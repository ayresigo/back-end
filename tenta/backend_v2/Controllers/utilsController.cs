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
        [HttpGet("/time/getUnixTime")]
        public IActionResult getUnixTime()
        {
            return Ok(DateTimeOffset.Now.ToUnixTimeSeconds());
        }
    }
}
