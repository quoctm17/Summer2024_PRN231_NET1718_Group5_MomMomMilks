using FE.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.CategoryManagement
{
    public class IndexModel : PageModel
    {
        public IEnumerable<ReadCategoryResponse> Categories{ get; set; }
        public void OnGet()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new CategoryIt.CategoryItClient(channel);

            GetAllCategoryResponse response = client.ListCategory(new GetAllCategoryRequest());
            Categories = response.Category;
        }
    }
}
