using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int OrderId { get; set; }
        public int StaffId { get; set; }
        public string Status { get; set; }
        public AppUser Staff { get; set; }
        public Order Order { get; set; }
    }
}
