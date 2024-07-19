﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using BlazorWebAssemblyUI.Services.Contracts;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ResponseModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorWebAssemblyUI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationService(
            IHttpClientFactory httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient.CreateClient("api");
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