using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public ICollection<Order> Orders { get; set; }
        public AppUser User { get; set; }

    }
}
