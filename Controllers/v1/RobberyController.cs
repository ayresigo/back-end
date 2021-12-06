using back_end.InputModel;
using back_end.Repositories;
using back_end.Services;
using back_end.Services.Interfaces;
using back_end.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RobberyController : ControllerBase
    {
        private readonly IRobberyService _robberyService;
        private readonly IAccountService _accountService;
        private readonly ICharacterMockService _characterMockService;
        private readonly IAuthService _authService;
        private readonly ITimeService _timeService;
        public RobberyController(IRobberyService robberyService, IAccountService accountService, ICharacterMockService characterMockService, IAuthService authService, ITimeService timeService)
        {
            _robberyService = robberyService;
            _accountService = accountService;
            _characterMockService = characterMockService;
            _authService = authService;
            _timeService = timeService;
        }

        [HttpGet("getRobbery")]
        public async Task<RobberyViewModel> getRobbery ([FromQuery] int id)
        {
            var robbery = await _robberyService.getRobbery(id);
            return robbery;
        }

        [HttpGet("getRobberies")]
        public async Task<List<RobberyViewModel>> getRobberies ([FromQuery] int status=1)
        {
            var robberies = await _robberyService.getRobberies(status);
            return robberies;
        }
        
        [HttpPost("startRobbery")]
        public async Task startRobbery ([FromBody] StartRobberyInputModel req)
        {
            await _robberyService.startRobbery(req);
        }
    }
}
