using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class CreateBatchDTO
    {
        public DateTime ImportDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public float ImportedPrice { get; set; }
        public int Quantity { get; set; }
        public int MilkId { get; set; }
    }
}
