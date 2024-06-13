using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class MilkImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool isMain { get; set; }
        public string PublicId { get; set; }
        public int MilkId { get; set; }
        public Milk Milk { get; set; }
    }
}
