using FE.Helpers;
using FE.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.MilkAgeManagement
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public UpdateMilkAgeRequest UpdateMilkAgeRequest { get; set; } = new UpdateMilkAgeRequest();
        public void OnGet(int id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new MilkAgeIt.MilkAgeItClient(channel);

            var request = new ReadMilkAgeRequest
            {
                Id = id,
            };
            var milkAge = client.ReadMilkAge(request);
            UpdateMilkAgeRequest.Id = milkAge.Id;
            UpdateMilkAgeRequest.Min = milkAge.Min;
            UpdateMilkAgeRequest.Max = milkAge.Max;
            UpdateMilkAgeRequest.Unit = milkAge.Unit;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new MilkAgeIt.MilkAgeItClient(channel);

                var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                var token = user.Token;

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };

                client.UpdateMilkAge(UpdateMilkAgeRequest, headers);
                return RedirectToPage("/admin/milkagemanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Update failed");
                return Page();
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new MilkAgeIt.MilkAgeItClient(channel);

                var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                var token = user.Token;

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };

                var request = new DeleteMilkAgeRequest { Id = UpdateMilkAgeRequest.Id };
                client.DeleteMilkAge(request, headers);
                return RedirectToPage("/admin/milkagemanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Update failed");
                return Page();
            }
        }
    }
}
