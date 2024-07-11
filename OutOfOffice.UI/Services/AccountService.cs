using OutOfOffice.Common.ViewModels;
using OutOfOffice.Common.ResponseModels;
using Newtonsoft.Json;
using System.Net.Http;

namespace OutOfOffice.UI.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }

        public async Task<AuthResponse?> LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", model);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<AuthResponse>(content);

                return deserializedResponse;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Login failed: {errorMessage}");
            }
        }

        public async Task LogoutAsync()
        {
            var response = await _httpClient.PostAsync("api/account/logout", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
