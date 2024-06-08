using FE.Models;
using Newtonsoft.Json;

namespace FE.Services
{
    public class BatchService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _clientFactory;

        public BatchService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("MyClient");
        }

        public async Task<List<Batch>> GetAllBatches()
        {
            var response = await _httpClient.GetAsync($"odata/Batches");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Batch>>(json);
            }
            else
            {
                return new List<Batch>();
            }
        }
    }
}
