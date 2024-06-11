using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.MilkCRUD;
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
		private readonly IMapper _mapper;

		public MilkController(IMilkService milkService, IMapper mapper)
        {
            _milkService = milkService;
			_mapper = mapper;
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
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateMilkDTO milkDto)
		{
			var milk = _mapper.Map<Milk>(milkDto);
			await _milkService.AddMilkAsync(milk);
			return Ok();
		}
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] UpdateMilkDTO milkDto)
		{
			var milk = _mapper.Map<Milk>(milkDto);
			await _milkService.UpdateMilkAsync(milk);
			return Ok();
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _milkService.DeleteMilkAsync(id);
			return Ok();
		}
	}
}
