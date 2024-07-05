using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Schedule
    {
        public int ShipperId { get; set; }
        public int OrderId { get; set; }
        public int TimeSlotId { get; set; }

        public Shipper Shipper { get; set; }
        public Order Order { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
