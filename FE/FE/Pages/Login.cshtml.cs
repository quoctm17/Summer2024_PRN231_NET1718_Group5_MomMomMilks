using FE.Helpers;
using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
                    if(account.Role == "Shipper")
                    {
                        return RedirectToPage("/shipper/index");
                    }
                    return RedirectToPage("/index");
                }
                else
                {
                    ModelState.AddModelError("Account", "Invalid account");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Account", "An error occured during login proccess");
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
