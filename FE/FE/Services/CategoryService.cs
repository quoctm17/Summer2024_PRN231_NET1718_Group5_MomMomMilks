using FE.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FE.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _clientFactory;

        public CategoryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("MyClient");
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("odata/Category");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Category>>(json);
            }
            else
            {
                return new List<Category>();
            }
        }
    }
}
