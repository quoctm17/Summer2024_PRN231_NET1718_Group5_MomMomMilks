using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Milk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string UserManual { get; set; }
        public string Warning { get; set; }
        public string PreserveInstructions { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int MilkAgeId { get; set; }
        public int SupplierId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public MilkAge MilkAge { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
