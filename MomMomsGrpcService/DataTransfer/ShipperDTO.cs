using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class ShipperDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public int AppUserId { get; set; }
    }
}
