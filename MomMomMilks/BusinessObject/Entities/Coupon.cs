using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        public DateTime EpiryDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int NumberOfUse { get; set; }
        public byte Status { get; set; }
        public ICollection<CouponUsageHistory> CouponUsageHistories { get; set; }
    }
}
