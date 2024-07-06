using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using Service.Services;

namespace MomMomMilks.Controllers
{
	[Route("odata/[controller]")]
	[ApiController]
	public class SupplierController : ODataController
	{
		private readonly ISupplierService _supplierService;
		private readonly IMapper _mapper;

		public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
			_supplierService = supplierService;
			_mapper = mapper;
		}
		[EnableQuery]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await _supplierService.GetAll();
			return Ok(result);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetSingleBatch(int id)
		{
			return Ok(await _supplierService.GetSingleSupplier(id));
		}
		[HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Post([FromBody] SupplierDTO supplierDto)
		{
			var supplier = _mapper.Map<Supplier>(supplierDto);
			var result = await _supplierService.CreateSupplier(supplier);
			if (!result) return BadRequest();
			return Ok(result);
		}
		[HttpPut]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put([FromBody] SupplierDTO supplierDTO)
		{
			var supplier = _mapper.Map<Supplier>(supplierDTO);
			var result = await _supplierService.UpdateSupplier(supplier);
			if (!result) return BadRequest();
			return Ok(result);
		}
		[HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id)
		{
			var result = await _supplierService.DeleteSupplier(id);
			if (!result) return BadRequest();
			return Ok(result);
		}
	}
}
