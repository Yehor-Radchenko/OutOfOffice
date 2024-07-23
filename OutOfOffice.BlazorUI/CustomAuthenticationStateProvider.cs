using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace OutOfOffice.BlazorUI
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = await _localStorageService.GetItemAsStringAsync("token");
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(_anonymous);
            }

            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            }
            else
            {
                await _localStorageService.RemoveItemAsync("token");
                MarkUserAsLoggedOut();
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private bool IsTokenExpired(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var expClaim = claims.FirstOrDefault(c => c.Type.Equals("exp"));
            if (expClaim != null)
            {
                var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));
                return expTime < DateTimeOffset.UtcNow;
            }
            return true; // If there's no expiration claim, consider the token as expired
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            ArgumentException.ThrowIfNullOrEmpty(jwt);

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            ArgumentException.ThrowIfNullOrEmpty(base64);

            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public void MarkUserAsLoggedOut()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }
    }
}
