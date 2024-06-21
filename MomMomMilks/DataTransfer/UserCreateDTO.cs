using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class UserCreateDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Username can only contain letters, digits, and spaces.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int Role { get; set; }
    }
}
