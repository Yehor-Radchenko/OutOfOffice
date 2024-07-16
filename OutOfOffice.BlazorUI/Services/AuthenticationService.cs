using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ResponseModels;

namespace OutOfOffice.BlazorUI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationService(
            IHttpClientFactory clientFactory,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorageService)
        {
            _httpClient = clientFactory.CreateClient("api");
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task<AuthResponse?> LoginAsync(LoginDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", model);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<AuthResponse>(content);

                if (deserializedResponse != null)
                {
                    await _localStorageService.SetItemAsync("token", deserializedResponse.Token);
                    StateHasChanged();
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
            await _localStorageService.RemoveItemAsync("token");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }

        private void StateHasChanged()
        {
            if (_authenticationStateProvider is CustomAuthenticationStateProvider customAuthProvider)
            {
                customAuthProvider.MarkUserStateAsChanged();
            }
        }
    }
}
