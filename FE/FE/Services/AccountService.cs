using FE.Models;
using FE.Pages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

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

        public async Task ForgotPasswordAsync(string email)
        {
            await _client.PostAsync("api/Account/ForgotPassword/" + email, null);
        }
        public async Task<bool> ResetPasswordAsync(ResetPassword resetPassword)
        {
            var response = await _client.PostAsJsonAsync("api/Account/ResetPassword", resetPassword);
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        public async Task<User> RegisterAsync(RegisterModel register)
        {
            var response = await _client.PostAsJsonAsync("api/Account/register", register);
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<User>();
            return user;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"odata/User?$filter=Id eq {userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    using (JsonDocument document = JsonDocument.Parse(json))
                    {
                        var root = document.RootElement;
                        var users = root.GetProperty("$values");

                        if (users.GetArrayLength() > 0)
                        {
                            var userJson = users[0].GetRawText();
                            var user = JsonConvert.DeserializeObject<User>(userJson);
                            return user;
                        }
                    }
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve user. Status code: {response.StatusCode}");
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
