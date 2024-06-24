namespace FE.Models.Shipper
{
    public class ShipperOrderDetail
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float TotalAmount { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerPhone { get; set; }
        public string PaymentTypeName { get; set; }
        public string OrderStatusName { get; set; }
        public string ScheduleTimeSlot { get; set; }
        public ShipperAddress Address { get; set; }
        public ICollection<ShipperOrderDetailItem> OrderDetails { get; set; }
    }
}
