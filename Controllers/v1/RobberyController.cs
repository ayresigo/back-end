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

        [HttpGet("getCharacterRobberiesLog")]
        public async Task<List<RobberyLogViewModel>> getCharacterRobberiesLog([FromQuery] int characterId)
        {
            var log = await _robberyService.getCharacterRobberyLogs(characterId);
            return log;
        }

        [HttpPost("startRobbery")]
        public async Task startRobbery([FromBody] StartRobberyInputModel req )
        {
            var token = new TokenInputModel
            {
                Token = req.token
            };
            var tokenObj = await _authService.retrieveToken(token);
            var sender = await _accountService.getAccount(tokenObj.address);
            var robbery = await _robberyService.getRobbery(req.robberyId);
            //check if sender is the owner of participants

            for (int i = 0; i < req.participants.Length; i++)
            {
                var character = await _characterMockService.getCharacter(req.participants[i].characterId);
                RobberyLogViewModel result = new RobberyLogViewModel
                {
                    characterId = character.id,
                    senderId = sender.id,
                    participants = req.participants.Length,
                    robberyId = robbery.id,
                    startMoney = sender.money,
                    startStamina = character.stamina,
                    startHealth = character.health,
                    startRespect = sender.respect,
                    startDate = DateTimeOffset.Now.ToUnixTimeSeconds(),

                    endDate = DateTimeOffset.Now.ToUnixTimeSeconds() + robbery.time,
                    endHealth = character.health,
                    endMoney = robbery.reward,
                    
                    //robberyStatus = 1,
                    //serverStatus = 1
                };

                await _characterMockService.editStatus(character.id, 2, robbery.time, await _timeService.getTime());
                await _robberyService.addLogs(result);
            }
        }

        [HttpGet("getRobbery")]
        public async Task<RobberyViewModel> getRobbery([FromQuery] int id)
        {
            var robbery = await _robberyService.getRobbery(id);
            return robbery;
        }

        [HttpGet("getRobberies")]
        public async Task<List<RobberyViewModel>> getRobberies([FromQuery] int status = 1)
        {
            var robberies = await _robberyService.getRobberies(status);
            return robberies;
        }
    }
}
