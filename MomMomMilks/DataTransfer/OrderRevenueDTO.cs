using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderRevenueDTO
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float TotalAmount { get; set; }
        public int PaymentTypeId { get; set; }
        public int TransactionId { get; set; }
        public IEnumerable<OrderDetailDTO> OrderDetails { get; set; }
    }
}
