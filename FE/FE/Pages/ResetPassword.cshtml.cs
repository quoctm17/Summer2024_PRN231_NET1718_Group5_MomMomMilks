using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly AccountService _accountService;

        public ResetPasswordModel(AccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty]
        public ResetPassword ResetPassword{ get; set; } = new ResetPassword();
        public void OnGet(string email)
        {
            ResetPassword.Email = email;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var result = await _accountService.ResetPasswordAsync(ResetPassword);
                if(result)
                {
                    return RedirectToPage("/login");
                } else
                {
                    ModelState.AddModelError("account", "Error occured while validating the process");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("account", ex.Message);
                return Page();
            }
        }
    }
}
