using FE.Models;
using Newtonsoft.Json;

namespace FE.Services
{
    public class OrderService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        public OrderService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("MyClient");
        }

        public async Task<List<OrderHistory>> GetAllOrderHistory(int userId)
        {
            var response = await _client.GetAsync($"odata/Order/OrderHistory({userId})");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<OrderHistory>>(json);
            }
            else
            {
                return new List<OrderHistory>();
            }
        }
    }
}
