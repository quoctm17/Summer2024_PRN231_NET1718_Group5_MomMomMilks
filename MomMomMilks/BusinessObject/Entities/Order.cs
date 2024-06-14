using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float TotalAmount { get; set; }
        public int BuyerId { get; set; }
        public int AddressId { get; set; }
        public int PaymentTypeId { get; set; }
        public int? TransactionId { get; set; }
        public int? ShipperId { get; set; }
        public int OrderStatusId { get; set; }
        public int TimeSlotId { get; set; }
        public Schedule Schedule { get; set; }
        public AppUser Buyer { get; set; }
        public AppUser Shipper { get; set; }
        public PaymentType PaymentType { get; set; }
        public Address Address { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection <Report> Reports { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<CouponUsageHistory> CouponUsageHistories { get; set; }

    }
}
