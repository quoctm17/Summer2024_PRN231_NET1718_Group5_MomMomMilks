using FE.Models;
using FE.Models.Shipper;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<List<OrderDetailHistory>> GetAllOrderDetailHistory(int orderId)
        {
            var response = await _client.GetAsync($"odata/Order/OrderDetail({orderId})");
            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<OrderDetailHistory>>(json);
            }
            else
            {
                return new List<OrderDetailHistory>();
            }
        }

        public async Task<List<ShipperOrder>> GetShipperOrders()
        {
            var response = await _client.GetAsync($"odata/Order/ShipperOrders");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ShipperOrder>>(json);
            }
            return null;
        }
        public async Task<ShipperOrderDetail> GetShipperOrder(int orderId)
        {
            var response = await _client.GetAsync($"odata/Order/ShipperOrders({orderId})");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ShipperOrderDetail>(json);
            }
            return null;
        }
        public async Task<bool> ConfirmShippedShipperOrder(int orderId)
        {
            var response = await _client.PutAsync($"odata/Order/ShipperOrders/confirm({orderId})", null);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
        public async Task<bool> ConfirmCancelledShipperOrder(int orderId)
        {
            var response = await _client.PutAsync($"odata/Order/ShipperOrders/cancel({orderId})", null);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
        public async Task<bool> Refund(List<Refund> refunds)
        {
            var json1 = JsonConvert.SerializeObject(refunds);

            // Create a StringContent object with the JSON payload
            var content = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"odata/Order/refund", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
    }
}
