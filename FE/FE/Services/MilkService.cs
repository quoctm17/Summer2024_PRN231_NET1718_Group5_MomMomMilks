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
        public async Task<List<Milk>> GetMilksByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"/odata/Milk?$filter=contains(name,'{name}')");
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
        public async Task<IEnumerable<Milk>> GetPaginatedMilks(int currentPage, int pageSize = 3)
        {
            var url = $"/odata/Milk?$orderby=Id&$skip={(currentPage - 1) * pageSize}&$top={pageSize}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var milks = JsonConvert.DeserializeObject<IEnumerable<Milk>>(json);
                return milks;
            }
            else
            {
                return Enumerable.Empty<Milk>();
            }
        }

        public async Task<IEnumerable<Milk>> GetMilkByCategoryAsync(int categoryId)
        {
            var url = $"/odata/Milk?$filter=CategoryId eq " + categoryId;

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var milks = JsonConvert.DeserializeObject<IEnumerable<Milk>>(json);
                return milks;
            }
            else
            {
                return Enumerable.Empty<Milk>();
            }
        }

        public async Task<int> GetTotalQuantityByMilkIdAsync(int milkId)
        {
            var response = await _httpClient.GetAsync($"odata/Batches/TotalQuantityByMilk/{milkId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(json);
            }
            return 0;
        }

    }
}
