using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Shipper
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int AppUserId { get; set; }
        public int DistrictId { get; set; }
        public AppUser AppUser { get; set; }
        public District District { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
