using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class CouponUsageHistory
    {
        public int OrderId { get; set; }
        public int CouponId { get; set; }
        public Order Order { get; set; }
        public Coupon Coupon { get; set; }
    }
}
