using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderId { get; set; }
        public double GrossAmount { get; set;}
        public Order Order { get; set; }
    }
}
