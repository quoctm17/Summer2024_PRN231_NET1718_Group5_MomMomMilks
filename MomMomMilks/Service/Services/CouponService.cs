using BusinessObject.Entities;
using DataTransfer;
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

        public async Task<List<CouponDTO>> GetAllCouponsAsync()
        {
            return await _couponRepository.GetAllCouponsAsync();
        }
        public async Task<List<CouponDTO>> GetAllAvailableCouponsAsync()
        {
            return await _couponRepository.GetAllAvailableCouponsAsync();
        }
        public async Task<List<CouponUsageDTO>> GetAllCouponUsagesAsync()
        {
            return await _couponRepository.GetAllCouponUsagesAsync();
        }

        public async Task<Coupon> GetCouponByIdAsync(int couponId)
        {
            return await _couponRepository.GetCouponByIdAsync(couponId);
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            await _couponRepository.AddCouponAsync(coupon);
        }
        public async Task AddOrderCouponAsync(string code, int orderId)
        {
            await _couponRepository.AddOrderCouponAsync(code, orderId);
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
        public async Task<bool> IsUseCoupon(int couponId, int userId)
        {
            return await _couponRepository.IsUseCoupon(couponId, userId);
        }
    }
}
