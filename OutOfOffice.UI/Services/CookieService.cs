using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

public class CookieService
{
    private readonly IJSRuntime _jsRuntime;

    public CookieService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetCookie(string name, string value, int expiresDays)
    {
        var expires = DateTime.UtcNow.AddDays(expiresDays).ToUniversalTime().ToString("R");
        await _jsRuntime.InvokeVoidAsync("setCookie", name, value, expires);
    }

    public async Task<string?> GetCookie(string name)
    {
        return await _jsRuntime.InvokeAsync<string>("getCookie", name);
    }

    public async Task DeleteCookie(string name)
    {
        await _jsRuntime.InvokeVoidAsync("deleteCookie", name);
    }
}
