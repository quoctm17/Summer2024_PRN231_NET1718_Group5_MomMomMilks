using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class FetchOrderRequestDTO
    {
        public int UserId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
