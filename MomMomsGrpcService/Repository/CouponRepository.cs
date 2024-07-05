
using BusinessObject.Entities;
using DataAccess.DAO;
using DataTransfer;
using Repository.Interface;
namespace Repository
{
    public class CouponRepository : ICouponRepository
    {
        public async Task<List<CouponDTO>> GetAllCouponsAsync()
        {
            return await CouponDAO.Instance.GetAllCouponsAsync();
        }
        public async Task<List<CouponUsageDTO>> GetAllCouponUsagesAsync()
        {
            return await CouponDAO.Instance.GetAllCouponUsagesAsync();
        }

        public async Task<Coupon> GetCouponByIdAsync(int couponId)
        {
            return await CouponDAO.Instance.GetCouponByIdAsync(couponId);
        }

        public async Task AddOrderCouponAsync(string code, int orderId)
        {
            await CouponDAO.Instance.AddOrderCouponAsync(code, orderId);
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            await CouponDAO.Instance.AddCouponAsync(coupon);
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            await CouponDAO.Instance.UpdateCouponAsync(coupon);
        }

        public async Task DeleteCouponAsync(int couponId)
        {
            await CouponDAO.Instance.DeleteCouponAsync(couponId);
        }

        public async Task UpdateCouponExpiryDate()
        {
            await CouponDAO.Instance.UpdateCouponExpiryDate();
        }
    }
}
