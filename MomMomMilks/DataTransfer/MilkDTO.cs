using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class MilkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string UserManual { get; set; }
        public string Warning { get; set; }
        public string PreserveInstructions { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string MilkAge { get; set; }
        public string Supplier { get; set; }
        public string ImageUrl { get; set; }
    }
}
