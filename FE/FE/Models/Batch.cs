namespace FE.Models
{
    public class Batch
    {
        public int Id { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public float ImportedPrice { get; set; }
        public int Quantity { get; set; }
        public int MilkId { get; set; }
        public Milk Milk { get; set; }
    }
}
