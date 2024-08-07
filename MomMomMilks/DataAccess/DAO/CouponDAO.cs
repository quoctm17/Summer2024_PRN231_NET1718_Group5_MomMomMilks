﻿using AutoMapper;
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
        public async Task<List<CouponUsageDTO>> GetAllCouponUsagesAsync()
        {
            var coupons = await _context.CouponUsageHistories.ToListAsync();
            return _mapper.Map<List<CouponUsageDTO>>(coupons);
        }
        public async Task<List<CouponDTO>> GetAllAvailableCouponAsync()
        {
            var coupons = await _context.Coupons.Where(c => c.Status == 1).ToListAsync();
            return _mapper.Map<List<CouponDTO>>(coupons);
        }

        public async Task<Coupon> GetCouponByIdAsync(int couponId)
        {
            return await _context.Coupons.FindAsync(couponId);
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            try
            {
                coupon.Status = 1;
                coupon.UpdateAt = DateTime.Now;
                coupon.CreateAt = DateTime.Now;
                await _context.Coupons.AddAsync(coupon);
                await _context.SaveChangesAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> IsUseCoupon(int couponId, int userId)
        {
            var coupon = await _context.Coupons.Where(c => c.Status == 1).Where(c => c.Id == couponId).FirstOrDefaultAsync();
            if (coupon != null)
            {
                var usageCoupon = await _context.CouponUsageHistories
                    .Include(c => c.Order)
                    .Where(c => c.Order.BuyerId == userId)
                    .Where(c => c.CouponId == coupon.Id)
                    .AnyAsync();
                return usageCoupon;
            }
            return true;
        }
        public async Task AddOrderCouponAsync(string code, int orderId)
        {
            if (code != null)
            {
                var coupon = await _context.Coupons.Where(c => c.Status == 1).Where(c => c.Code == code).FirstOrDefaultAsync();
                var order = await _context.Orders.Where(c => c.Id == orderId).FirstOrDefaultAsync();
                var userId = order.BuyerId;
                bool? usageCoupon = true;
                usageCoupon = await IsUseCoupon(coupon.Id, userId);

                if (coupon != null)
                {
                    if ((bool)!usageCoupon)
                    {
                        var useCoupon = new CouponUsageHistory()
                        {
                            OrderId = orderId,
                            CouponId = coupon.Id
                        };
                        await _context.CouponUsageHistories.AddAsync(useCoupon);
                        await _context.SaveChangesAsync();

                        coupon.NumberOfUse -= 1;
                        if (coupon.NumberOfUse <= 0)
                        {
                            coupon.Status = 0;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            try
            {
                var existingCoupon = await _context.Coupons.FindAsync(coupon.Id);
                if (existingCoupon != null)
                {
                    _context.Entry(existingCoupon).State = EntityState.Detached;
                }

                coupon.UpdateAt = DateTime.Now;

                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
