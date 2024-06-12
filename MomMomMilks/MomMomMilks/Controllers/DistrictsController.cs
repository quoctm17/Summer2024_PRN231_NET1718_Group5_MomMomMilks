using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interface;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class DistrictsController : ODataController
    {
        private readonly IDistrictService _districtService;
        public DistrictsController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_districtService.GetAllDistricts());
        }
    }
}
