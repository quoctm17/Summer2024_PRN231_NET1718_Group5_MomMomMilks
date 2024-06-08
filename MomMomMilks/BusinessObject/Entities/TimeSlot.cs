using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class TimeSlot
    {
        //"Morning: 7-10h", "Afternoon: 12h - 15h", "Evening: 17h - 20h"
        public int Id { get; set; }
        public string Name { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }

}
