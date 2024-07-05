using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public int MilkId { get; set; }
        public Milk Milk { get; set; }
        public AppUser User { get; set; }
    }
}
