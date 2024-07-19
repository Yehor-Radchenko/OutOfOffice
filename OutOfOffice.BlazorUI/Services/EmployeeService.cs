namespace OutOfOffice.BlazorUI.Services;

using OutOfOffice.BlazorUI.Services.Contracts;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Enums;
using OutOfOffice.Common.ViewModels.Employee;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;

    public EmployeeService(IHttpClientFactory httpClientFactory, string authToken)
    {
        _httpClient = httpClientFactory.CreateClient("api");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
    }

    public async Task<List<TableEmployeeViewModel>> GetTableDataAsync(string? search = null)
    {
        var url = search != null ? $"api/employee/table-data?search={search}" : "api/employee/table-data";
        return await _httpClient.GetFromJsonAsync<List<TableEmployeeViewModel>>(url);
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<FullEmployeeViewModel>($"api/employee/{id}");
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAboutAuthenticatedEmployeeAsync()
    {

        return await _httpClient.GetFromJsonAsync<FullEmployeeViewModel>("api/employee");
    }

    public async Task<int> CreateEmployeeAsync(EmployeeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/employee", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> UpdateEmployeeAsync(int id, EmployeeDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/employee/update/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> ChangeStatusAsync(int id, EmployeeStatus status)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/employee/changeStatus/{id}", status);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}