using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
	[Route("odata/[controller]")]
	[ApiController]
	public class BrandsController : ODataController
	{
		private readonly IBrandService _brandService;
		private readonly IMapper _mapper;

		public BrandsController(IBrandService brandService, IMapper mapper)
        {
			_brandService = brandService;
			_mapper = mapper;
		}
        [EnableQuery]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await _brandService.GetAll();
			return Ok(result);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetSingleBrand(int id)
		{
			return Ok(await _brandService.GetSingleBrand(id));
		}
		[HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody] BrandDTO brandDTO)
		{
			var supplier = _mapper.Map<Brand>(brandDTO);
			var result = await _brandService.CreateBrand(supplier);
			if (!result) return BadRequest();
			return Ok(result);
		}
		[HttpPut]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put([FromBody] BrandDTO brandDTO)
		{
			var supplier = _mapper.Map<Brand>(brandDTO);
			var result = await _brandService.UpdateBrand(supplier);
			if (!result) return BadRequest();
			return Ok(result);
		}
		[HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id)
		{
			var result = await _brandService.DeleteBrand(id);
			if (!result) return BadRequest();
			return Ok(result);
		}
	}
}
