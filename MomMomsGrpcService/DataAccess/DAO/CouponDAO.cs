using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class CouponDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private static CouponDAO instance;

        public CouponDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
        }

        public static CouponDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CouponDAO();
                }
                return instance;
            }
        }

        public async Task<List<CouponDTO>> GetAllCouponsAsync()
        {
            var coupons = await _context.Coupons.Where(c => c.Status == 1).ToListAsync();
            return _mapper.Map<List<CouponDTO>>(coupons);
        }
        public async Task<List<CouponUsageDTO>> GetAllCouponUsagesAsync()
        {
            var coupons = await _context.CouponUsageHistories.ToListAsync();
            return _mapper.Map<List<CouponUsageDTO>>(coupons);
        }

        public async Task<Coupon> GetCouponByIdAsync(int couponId)
        {
            return await _context.Coupons.FindAsync(couponId);
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
        }
        public async Task AddOrderCouponAsync(string code, int orderId)
        {
            var coupon = await _context.Coupons.Where(c => c.Code == code).FirstOrDefaultAsync();
            var useCoupon = new CouponUsageHistory()
            {
                OrderId = orderId,
                CouponId = coupon.Id
            };
            await _context.CouponUsageHistories.AddAsync(useCoupon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCouponAsync(int couponId)
        {
            var coupon = await _context.Coupons.FindAsync(couponId);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCouponExpiryDate()
        {
            var expiredCoupons = await _context.Coupons.Where(c => c.Status == 1).Where(c => c.EpiryDate < DateTime.Now).ToListAsync();
            foreach (var coupon in expiredCoupons)
            {
                coupon.Status = 0;
            }
            await _context.SaveChangesAsync();
        }
    }
}
