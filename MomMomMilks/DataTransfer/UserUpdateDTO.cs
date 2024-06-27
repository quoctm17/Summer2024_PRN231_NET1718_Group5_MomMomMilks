using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]*$", ErrorMessage = "Username can only contain letters (including Vietnamese characters) and spaces.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [EmailAddress]
        public string Email { get; set; }
        public byte Status { get; set; }
        public int Role { get; set; }
    }
}
