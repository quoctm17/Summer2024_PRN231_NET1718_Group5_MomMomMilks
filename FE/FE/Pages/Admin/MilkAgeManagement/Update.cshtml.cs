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

                client.UpdateMilkAge(UpdateMilkAgeRequest);
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

                var request = new DeleteMilkAgeRequest { Id = UpdateMilkAgeRequest.Id };
                client.DeleteMilkAge(request);
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
