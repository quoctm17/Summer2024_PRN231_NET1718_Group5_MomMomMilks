using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderDetailSimpleDto
    {
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int MilkId { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public float Price { get; set; }
        public int Total { get; set; }
        public string Status { get; set; }

    }
}
