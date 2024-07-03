using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderDetailHistoryDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string MilkName { get; set; }
        public int Discount { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
