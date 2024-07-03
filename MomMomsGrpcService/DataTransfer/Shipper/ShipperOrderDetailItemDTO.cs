using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Shipper
{
    public class ShipperOrderDetailItemDTO
    {
        public int Id { get; set; }
        public string MilkName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
