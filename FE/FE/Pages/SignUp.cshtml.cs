using FE.Helpers;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FE.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly AccountService _accountService;

        public SignUpModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public RegisterModel Register { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = await _accountService.RegisterAsync(Register);
                if (user == null)
                {
                    ModelState.AddModelError("Account", "An error occured while registering. Try register another email");
                    return Page();
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "user", user);
                return RedirectToPage("/index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Account", "An error occured while registering. Try register another email");
            }
            return Page();
        }
    }

    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmedPassword { get; set; }

    }
}
