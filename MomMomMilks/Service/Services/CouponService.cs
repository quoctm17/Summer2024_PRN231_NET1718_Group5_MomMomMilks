using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<List<Coupon>> GetAllCouponsAsync()
        {
            return await _couponRepository.GetAllCouponsAsync();
        }

        public async Task<Coupon> GetCouponByIdAsync(int couponId)
        {
            return await _couponRepository.GetCouponByIdAsync(couponId);
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            await _couponRepository.AddCouponAsync(coupon);
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            await _couponRepository.UpdateCouponAsync(coupon);
        }

        public async Task DeleteCouponAsync(int couponId)
        {
            await _couponRepository.DeleteCouponAsync(couponId);
        }

        public async Task UpdateCouponExpiryDate()
        {
            await _couponRepository.UpdateCouponExpiryDate();
        }
    }
}
