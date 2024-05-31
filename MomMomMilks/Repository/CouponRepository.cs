using DataAccess.DAO.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ICouponDAO _couponDAO;
        public CouponRepository(ICouponDAO couponDAO)
        {
            _couponDAO = couponDAO;
        }
        public async Task UpdateCouponExpiryDate()
        {
            await _couponDAO.UpdateCouponExpiryDate();
        }
    }
}
