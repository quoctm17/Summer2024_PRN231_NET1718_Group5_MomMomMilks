using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilkController : ControllerBase
    {
        private readonly IMilkRepository _milkRepository;
        public MilkController(IMilkRepository milkRepository)
        {
            _milkRepository = milkRepository;
        }

        [HttpGet("milks")]
        public async Task<IActionResult> GetAllMilk()
        {
            return Ok(await _milkRepository.GetAllMilk());
        }
    }
}
