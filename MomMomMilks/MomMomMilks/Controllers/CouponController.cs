using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using Service.Services;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class CouponController : ODataController
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;

        public CouponController(ICouponService couponService, IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return Ok(coupons);
        }
        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var category = await _couponService.GetCouponByIdAsync(key);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CouponDTO create)
        {
            var coupon = _mapper.Map<Coupon>(create);
            await _couponService.AddCouponAsync(coupon);
            return Created(coupon);
        }
        [HttpPost("usage")]
        public async Task<IActionResult> Post([FromQuery] string code, int orderId)
        {
            await _couponService.AddOrderCouponAsync(code, orderId);
            return Ok();
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] CouponDTO update)
        {
            var coupon = _mapper.Map<Coupon>(update);
            await _couponService.UpdateCouponAsync(coupon);

            return Updated(update);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var existingCoupon = await _couponService.GetCouponByIdAsync(key);
            if (existingCoupon == null)
            {
                return NotFound();
            }

            await _couponService.DeleteCouponAsync(key);
            return NoContent();
        }
    }
}
