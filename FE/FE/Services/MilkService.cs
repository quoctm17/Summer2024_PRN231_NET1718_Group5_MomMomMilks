using FE.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace FE.Services
{
    public class MilkService
    {
        private readonly HttpClient _httpClient;

        public MilkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
