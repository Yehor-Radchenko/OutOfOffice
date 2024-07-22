using OutOfOffice.BlazorUI.Services.Contracts;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Enums;
namespace OutOfOffice.BlazorUI.Services;

using OutOfOffice.Common.ViewModels.Employee;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public EmployeeService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
    {
        _httpClient = httpClientFactory.CreateClient("API");
        _localStorageService = localStorageService;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var authToken = await _localStorageService.GetItemAsStringAsync("token");
        if (!string.IsNullOrEmpty(authToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<List<TableEmployeeViewModel>> GetTableDataAsync(string? search = null)
    {
        await SetAuthorizationHeaderAsync();
        var url = search != null ? $"api/employee/table-data?search={search}" : "api/employee/table-data";
        return await _httpClient.GetFromJsonAsync<List<TableEmployeeViewModel>>(url);
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAsync(int id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<FullEmployeeViewModel>($"api/employee/{id}");
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAboutAuthenticatedEmployeeAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<FullEmployeeViewModel>("api/employee/profile");
    }

    public async Task<int> CreateEmployeeAsync(EmployeeDto dto)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/employee", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> UpdateEmployeeAsync(int id, EmployeeDto dto)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/employee/update/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> ChangeStatusAsync(int id, EmployeeStatus status)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/employee/changeStatus/{id}", status);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}
