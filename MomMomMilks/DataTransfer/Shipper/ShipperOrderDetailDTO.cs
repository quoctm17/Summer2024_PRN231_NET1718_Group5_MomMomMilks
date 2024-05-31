using BusinessObject.Entities;

namespace DataTransfer.Shipper
{
    public class ShipperOrderDetailDTO
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float TotalAmount { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string PaymentTypeName { get; set; }
        public string OrderStatusName { get; set; }
        public string ScheduleTimeSlot { get; set; }
        public Address Address { get; set; }
        public ICollection<ShipperOrderDetailItemDTO> OrderDetails { get; set; }
    }
}
