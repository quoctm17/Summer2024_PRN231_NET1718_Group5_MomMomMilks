using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.BrandManagement
{
    public class IndexModel : PageModel
    {
        public IEnumerable<ReadBrandResponse> Brands { get; set; }
        public void OnGet()
        {
			var channel = GrpcChannel.ForAddress("https://localhost:7269");
			var client = new BrandIt.BrandItClient(channel);

			GetAllResponse response = client.ListBrand(new GetAllRequest());
            Brands = response.Brand;

		}
	}
}
