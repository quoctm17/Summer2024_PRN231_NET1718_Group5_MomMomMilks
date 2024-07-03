using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.MilkAgeManagement
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IEnumerable<ReadMilkAgeResponse> MilkAges { get; set; }
        public void OnGet()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new MilkAgeIt.MilkAgeItClient(channel);

            var response = client.ListMilkAge(new GetMilkAgeAllRequest());
            MilkAges = response.MilkAge;
        }
    }
}
