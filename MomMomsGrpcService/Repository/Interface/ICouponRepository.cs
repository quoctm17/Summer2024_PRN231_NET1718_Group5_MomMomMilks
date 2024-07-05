using BusinessObject.Entities;
using DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICouponRepository
    {
        Task<List<CouponDTO>> GetAllCouponsAsync();
        Task<List<CouponUsageDTO>> GetAllCouponUsagesAsync();
        Task<Coupon> GetCouponByIdAsync(int couponId);
        Task AddCouponAsync(Coupon coupon);
        Task UpdateCouponAsync(Coupon coupon);
        Task DeleteCouponAsync(int couponId);
        Task UpdateCouponExpiryDate();
        Task AddOrderCouponAsync(string code, int orderId);
    }
}
