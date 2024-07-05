using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class MilkAge
    {
        public int Id { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Unit { get; set; }
        public ICollection<Milk> Milks { get; set; }
    }
}
