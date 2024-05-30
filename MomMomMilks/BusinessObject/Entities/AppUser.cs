using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public byte Status { get; set; }
        public int Point { get; set; }
        public Shipper Shipper { get; set; }
        public Cart Cart { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Report> Reports { get; set; }

    }
}
