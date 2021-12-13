using cryminals.Models.ViewModels;
using cryminals.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class robberiesController : ControllerBase
    {
        private readonly IRobberyRepository _robberyRepository;

        public robberiesController(IRobberyRepository robberyRepository)
        {
            _robberyRepository = robberyRepository;
        }

        [HttpGet("getRobbery")]
        public async Task<IActionResult> getRobbery(int id)
        {
            try
            {
                var robbery = await _robberyRepository.getRobbery(id);
                return Ok(robbery);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 

        [HttpGet("getRobberies")]
        public async Task<IActionResult> getRobberies(int status = 1)
        {
            try
            {
                var robberies = await _robberyRepository.getRobberies(status);
                return Ok(robberies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
