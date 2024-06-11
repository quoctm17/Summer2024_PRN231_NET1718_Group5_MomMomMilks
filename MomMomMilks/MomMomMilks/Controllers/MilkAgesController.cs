using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.Interfaces;
using Service.Services;

namespace MomMomMilks.Controllers
{
	[Route("odata/[controller]")]
	[ApiController]
	public class MilkAgesController : Controller
	{
		private readonly IMilkAgeService _milkAgeService;
		private readonly IMapper _mapper;

		public MilkAgesController(IMilkAgeService milkAgeService, IMapper mapper)
        {
			_milkAgeService = milkAgeService;
			_mapper = mapper;
		}
		[EnableQuery]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await _milkAgeService.GetAll();
			return Ok(result);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetSingleBatch(int id)
		{
			return Ok(await _milkAgeService.GetSingleMilkAge(id));
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] MilkAgeDTO milkAgeDTO)
		{
			var supplier = _mapper.Map<MilkAge>(milkAgeDTO);
			var result = await _milkAgeService.CreateMilkAge(supplier);
			if (!result) return BadRequest();
			return Ok(result);
		}
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] MilkAgeDTO milkAgeDTO)
		{
			var supplier = _mapper.Map<MilkAge>(milkAgeDTO);
			var result = await _milkAgeService.UpdateMilkAge(supplier);
			if (!result) return BadRequest();
			return Ok(result);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _milkAgeService.DeleteMilkAge(id);
			if (!result) return BadRequest();
			return Ok(result);
		}
	}
}
