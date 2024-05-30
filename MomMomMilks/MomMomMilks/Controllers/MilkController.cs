using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilkController : ControllerBase
    {
        private readonly IMilkService _milkService;
        public MilkController(IMilkService milkService)
        {
            _milkService = milkService;
        }

        [HttpGet("milks")]
        public async Task<IActionResult> GetAllMilk()
        {
            return Ok(await _milkService.GetAllMilk());
        }
    }
}
