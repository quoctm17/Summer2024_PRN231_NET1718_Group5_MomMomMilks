using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Manager
{
    public class ManagerOrderDTO
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float TotalAmount { get; set; }
        public string BuyerName { get; set; }
        public string PaymentTypeName { get; set; }
        public string OrderStatusName { get; set; }
        public string ScheduleTimeSlot { get; set; }
    }
}
