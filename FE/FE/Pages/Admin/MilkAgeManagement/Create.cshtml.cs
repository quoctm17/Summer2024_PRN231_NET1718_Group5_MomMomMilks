using FE.Helpers;
using FE.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.MilkAgeManagement
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateMilkAgeRequest CreateMilkAgeRequest { get; set; } = new CreateMilkAgeRequest();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(CreateMilkAgeRequest.Unit) || CreateMilkAgeRequest.Min == 0 | CreateMilkAgeRequest.Max == 0)
                {
                    ModelState.AddModelError("Errors", "Fill in all fields");
                }
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new MilkAgeIt.MilkAgeItClient(channel);

                var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                var token = user.Token;

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };

                var response = client.CreateMilkAge(CreateMilkAgeRequest, headers);
                return RedirectToPage("/admin/milkagemanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Create failed");
                return Page();
            }
        }
    }
}
