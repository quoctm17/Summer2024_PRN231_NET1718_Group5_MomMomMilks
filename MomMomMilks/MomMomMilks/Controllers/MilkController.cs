using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.Interface;
using System.Threading.Tasks;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class MilkController : ODataController
    {
        private readonly IMilkRepository _milkRepository;

        public MilkController(IMilkRepository milkRepository)
        {
            _milkRepository = milkRepository;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var milks = await _milkRepository.GetAllMilk();
            return Ok(milks);
        }
    }
}
