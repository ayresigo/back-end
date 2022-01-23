
using cryminals.Repositories.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class itemsController : ControllerBase
    {
        public IItemRepository _itemRepository;

        public itemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet("getItem")]
        public async Task<IActionResult> getItem(int id)
        {
            try
            {
                var item = await _itemRepository.getItemDB(id);
                if (item != null)
                {
                    return Ok(item);
                }
                else
                {
                    return NotFound("Not found!");
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
