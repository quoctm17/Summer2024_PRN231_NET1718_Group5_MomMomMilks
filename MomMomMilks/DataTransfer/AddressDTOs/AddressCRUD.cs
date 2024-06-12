using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.AddressDTOs
{
    public class AddressCRUD
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public int WardId { get; set; }
        public int DistrictId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
