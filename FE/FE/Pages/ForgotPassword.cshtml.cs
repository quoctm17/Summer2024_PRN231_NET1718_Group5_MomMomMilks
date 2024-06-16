using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly AccountService _accountService;

        public ForgotPasswordModel(AccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty]
        public string Email { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("Account", "Invalid email");
                return Page();
            }
            try
            {
                await _accountService.ForgotPasswordAsync(Email);
                return RedirectToPage("/resetpassword", new {
                    email = Email
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Account", "Invalid email");
                return Page();
            }
        }
    }
}
