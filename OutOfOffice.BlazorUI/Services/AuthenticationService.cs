using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ResponseModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OutOfOffice.BlazorUI.Services.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace OutOfOffice.BlazorUI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _factory;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationService(
            IHttpClientFactory httpClientFactory,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorageService)
        {
            _factory = httpClientFactory;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task<AuthResponse?> LoginAsync(LoginDto model)
        {
            var response = await _factory.CreateClient("API").PostAsJsonAsync("api/account/login", model);

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
            await _localStorageService.RemoveItemAsync("token");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }
    }
}
