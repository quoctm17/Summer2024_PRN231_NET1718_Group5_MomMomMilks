using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int UserId { get; set; }
        public int MilkId { get; set; }
        public AppUser AppUser { get; set; }
        public Milk Milk { get; set; }
    }
}
