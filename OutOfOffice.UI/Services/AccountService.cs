using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using OutOfOffice.Common.ViewModels;
using System.Net.Http.Headers;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace OutOfOffice.UI.Services;

public class AccountService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AccountService(IHttpClientFactory clientFactory, IJSRuntime jsRuntime, AuthenticationStateProvider authenticationStateProvider)
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

    public async Task<string> LoginAsync(LoginViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/account/login", model);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = 'jwt-token={token}; path=/;'");

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt"));
            NotifyAuthenticationStateChanged(authenticatedUser);

            return token;

            return token;
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

        await _jsRuntime.InvokeVoidAsync("document.cookie", "jwt-token=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/");
    }

    private void NotifyAuthenticationStateChanged(ClaimsPrincipal user)
    {
        var authStateTask = Task.FromResult(new AuthenticationState(user));
        (_authenticationStateProvider).NotifyAuthenticationStateChanged(authStateTask);
    }
}