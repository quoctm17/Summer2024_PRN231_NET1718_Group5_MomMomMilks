using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.Interface;
using Service.Interfaces;
using Service.Services;
using System.Threading.Tasks;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class MilkController : ODataController
    {
        private readonly IMilkService _milkService;
        public MilkController(IMilkService milkService)
        {
            _milkService = milkService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _milkService.GetAllMilk());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var milk = await _milkService.GetMilkById(key);
            if (milk == null)
            {
                return NotFound();
            }
            return Ok(milk);
        }
    }
}
