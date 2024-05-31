using FE.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace FE.Services
{
    public class MilkService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _clientFactory;

        public MilkService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("MyClient");
        }

        public async Task<List<Milk>> GetMilksAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5215/odata/Milk");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Milk>>(json);
            }
            else
            {
                return new List<Milk>();
            }
        }
        public async Task<Milk> GetMilkByIdAsync(int milkId)
        {
            var response = await _httpClient.GetAsync($"odata/Milk/{milkId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Milk>(json);
            }
            else
            {
                return new Milk();
            }
        }
    }
}
