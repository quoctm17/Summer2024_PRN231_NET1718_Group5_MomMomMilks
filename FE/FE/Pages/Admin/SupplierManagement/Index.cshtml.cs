using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.SupplierManagement
{
    public class IndexModel : PageModel
    {

        public IEnumerable<ReadSupplierResponse> Suppliers{ get; set; }
        public void OnGet()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new SupplierIt.SupplierItClient(channel);

            var response = client.ListSupplier(new GetAllSupplierRequest());
            Suppliers = response.Supplier;
        }
    }
}
