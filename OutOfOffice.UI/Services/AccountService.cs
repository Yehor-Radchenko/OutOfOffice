using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using OutOfOffice.Common.ViewModels;
using System.Net.Http.Headers;
using Microsoft.JSInterop;
using System.Security.Claims;
using OutOfOffice.Common.ResponseModels;
using Newtonsoft.Json;

namespace OutOfOffice.UI.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

        public AccountService(IHttpClientFactory clientFactory, IJSRuntime jsRuntime, CustomAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = clientFactory.CreateClient("api");
            _jsRuntime = jsRuntime;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> IsAuthenticated()
        {
            var token = await _jsRuntime.InvokeAsync<string>("eval", "document.cookie.match('(^|;)\\s*jwt-token\\s*=\\s*([^;]+)')?.pop()");
            return !string.IsNullOrEmpty(token);
        }

        public async Task<AuthResponse?> LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", model);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<AuthResponse>(content);

                if (deserializedResponse != null)
                {
                    _authenticationStateProvider.AuthenticateUser(deserializedResponse.Token);
                }

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

            _authenticationStateProvider.LogoutUser();
        }
    }
}
