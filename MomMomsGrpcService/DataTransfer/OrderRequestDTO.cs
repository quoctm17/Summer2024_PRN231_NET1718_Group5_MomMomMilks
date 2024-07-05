using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class OrderRequestDto
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }

}
