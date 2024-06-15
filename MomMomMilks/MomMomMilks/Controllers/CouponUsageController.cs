using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponUsageController : ODataController
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;

        public CouponUsageController(ICouponService couponService, IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var coupons = await _couponService.GetAllCouponUsagesAsync();
            return Ok(coupons);
        }
        /*[EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var category = await _couponService.GetCouponByIdAsync(key);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }*/

        [HttpPost]
        public async Task<IActionResult> Post(string code, int orderId)
        {
            await _couponService.AddOrderCouponAsync(code, orderId);
            return Ok();
        }
    }
}
