using FE.Helpers;
using FE.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.CategoryManagement
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateCategoryRequest CreateCategoryRequest { get; set; } = new CreateCategoryRequest();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(CreateCategoryRequest.Name))
                {
                    ModelState.AddModelError("Errors", "Fill in all fields");
                }
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new CategoryIt.CategoryItClient(channel);
                var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                var token = user.Token;

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };
                var response = client.CreateCategory(CreateCategoryRequest, headers);
                return RedirectToPage("/admin/categorymanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Create failed");
                return Page();
            }
        }
    }
}
