using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace OutOfOffice.BlazorUI
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;

        public AuthHeaderHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authToken = await _localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(authToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
