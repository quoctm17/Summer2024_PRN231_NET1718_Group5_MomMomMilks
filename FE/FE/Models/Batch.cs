using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class Batch
    {
        public int Id { get; set; }
        [Required]
        public DateTime ImportDate { get; set; }
        [Required]
        public DateTime ExpiredDate { get; set; }
        [Required]
        public float ImportedPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int MilkId { get; set; }
        public Milk? Milk { get; set; }
    }
}
