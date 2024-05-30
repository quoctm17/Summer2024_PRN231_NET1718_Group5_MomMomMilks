using FE.Models;

namespace FE.Services
{
    public class AccountService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        public AccountService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("MyClient");
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var response = await _client.PostAsJsonAsync("api/Account/login", new
            {
                email,
                password
            });
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<User>();
            return user;
        }
    }
}
