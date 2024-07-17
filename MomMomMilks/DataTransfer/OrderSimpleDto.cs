using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderSimpleDto
    {
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public int BuyerId { get; set; }
        public int AddressId { get; set; }
        public int PaymentTypeId { get; set; }
        public int OrderStatusId { get; set; }
        public int TimeSlotId { get; set; }
        public string? Code { get; set; }
        public List<OrderDetailSimpleDto> OrderDetails { get; set; }
    }

}
