using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class ResetPassword
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password must be the same.")]
        public string ConfirmPassword { get; set; }
        [Required]

        public string Code { get; set; }
    }
}
