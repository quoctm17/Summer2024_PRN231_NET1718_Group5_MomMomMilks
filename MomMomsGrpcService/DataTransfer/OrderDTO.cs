using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float TotalAmount { get; set; }
        public int BuyerId { get; set; }
        public int AddressId { get; set; }
        public int PaymentTypeId { get; set; }
        public int TransactionId { get; set; }
        public int ShipperId { get; set; }
        public long PaymentOrderCode {  get; set; }
        public int OrderStatusId { get; set; }
        public ICollection<OrderDetailDTO> OrderDetail { get; set; }
    }
}
