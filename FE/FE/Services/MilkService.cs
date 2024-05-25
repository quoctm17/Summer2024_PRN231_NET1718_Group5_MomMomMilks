using FE.Models;
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
            var response = await _httpClient.GetAsync("http://localhost:5215/api/Milk/milks");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Milk>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
