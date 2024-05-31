using AutoMapper;
using DataAccess.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CouponDAO : ICouponDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CouponDAO(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
