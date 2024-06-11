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
        public async Task<Batch> GetSingleBatch(int id)
        {
            var response = await _httpClient.GetAsync($"odata/Batches/" + id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Batch>(json);
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateBatch(Batch batch)
        {
            var response = await _httpClient.PostAsJsonAsync($"odata/Batches", batch);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
        public async Task<bool> UpdateBatch(Batch batch)
        {
            var response = await _httpClient.PutAsJsonAsync($"odata/Batches", batch);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
        public async Task<bool> DeleteBatch(int id)
        {
            var response = await _httpClient.DeleteAsync($"odata/Batches/" + id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
    }
}
