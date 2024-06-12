using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Customer
{
    public class UserInformationModel : PageModel
    {
        private readonly AccountService _accountService;
        public UserInformationModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public User User { get; set; }
        public async Task<IActionResult> OnGet(int userId)
        {
            User = await _accountService.GetUserAsync(userId);
            return Page();
        }
    }
}
