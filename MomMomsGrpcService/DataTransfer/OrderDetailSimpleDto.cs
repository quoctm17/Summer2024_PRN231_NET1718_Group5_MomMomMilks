using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderDetailSimpleDto
    {
        public int MilkId { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public float Price { get; set; }
        public int Total { get; set; }
    }
}
