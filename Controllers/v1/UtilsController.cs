using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]


    public class UtilsController : ControllerBase
    {
        private readonly ITimeService _timeService;
        public UtilsController(ITimeService timeService)
        {
            _timeService = timeService;
        }

        [HttpGet("getTime")]
        public Task<long> getTime()
        {
            return _timeService.getTime();
        }
    }
}
