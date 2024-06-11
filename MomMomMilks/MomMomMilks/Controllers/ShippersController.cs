using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using Service.Services;

namespace MomMomMilks.Controllers
{
	[Route("odata/[controller]")]
	[ApiController]
	public class ShippersController : ODataController
	{
		private readonly IShipperService _shipperService;

		public ShippersController(IShipperService shipperService)
        {
			_shipperService = shipperService;
		}
		[EnableQuery]
		[HttpGet("available")]
		public async Task<IActionResult> GetAvailableShippers()
		{
			var result = await _shipperService.GetAvailableShippers();
			return Ok(result);
		}
	}
}
