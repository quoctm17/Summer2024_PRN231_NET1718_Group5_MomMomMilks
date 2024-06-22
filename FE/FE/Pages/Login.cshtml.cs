using FE.Helpers;
using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AccountService _accountService;

        public LoginModel(AccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty]
        public Credential Credential { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                var account = await _accountService.LoginAsync(Credential.Email, Credential.Password);
                if (account != null)
                {
                    HttpContext.Session.SetObjectAsJson("user", account);
                    HttpContext.Session.SetString("token", account.Token);
                    if(account.Role == "Admin")
                    {
                        return RedirectToPage("/admin/index");
                    } else if(account.Role == "Manager")
                    {
						return RedirectToPage("/manager/managerorder/index");
					} else if(account.Role == "Shipper")
                    {
                        return RedirectToPage("/shipper/index");
                    }
					// Return a script to set the token in sessionStorage
					var script = $"<script>sessionStorage.setItem('token', '{account.Token}'); window.location.href = '/index';</script>";
                    return Content(script, "text/html");
                }
                else
                {
                    ModelState.AddModelError("Account", "Invalid account");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Account", "An error occurred during the login process");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
