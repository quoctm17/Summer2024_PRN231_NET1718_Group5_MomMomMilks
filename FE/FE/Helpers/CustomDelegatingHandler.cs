using FE.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FE.Helpers
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
