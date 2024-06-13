using AutoMapper;
using BusinessObject.Entities;
using DataAccess.AutoMapperProfile;
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
            var coupons = await _context.Coupons.ToListAsync();
            return _mapper.Map<List<CouponDTO>>(coupons);
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
            var expiredCoupons = await _context.Coupons.Where(c => c.EpiryDate < DateTime.Now).ToListAsync();
            foreach (var coupon in expiredCoupons)
            {
                coupon.Status = 0;
            }
            await _context.SaveChangesAsync();
        }
    }
}
