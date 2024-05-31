namespace FE.Models.Shipper
{
    public class ShipperOrderDetailItem
    {
        public int Id { get; set; }
        public string MilkName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
