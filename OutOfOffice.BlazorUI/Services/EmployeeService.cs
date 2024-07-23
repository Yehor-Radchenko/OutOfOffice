using OutOfOffice.BlazorUI.Services.Contracts;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Enums;
using OutOfOffice.Common.ViewModels.Employee;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace OutOfOffice.BlazorUI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EmployeeService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<TableEmployeeViewModel>> GetTableDataAsync(string? search = null)
    {
        var url = search != null ? $"api/employee/table-data?search={search}" : "api/employee/table-data";
        return await _httpClientFactory.CreateClient("API").GetFromJsonAsync<List<TableEmployeeViewModel>>(url);
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAsync(int id)
    {
        return await _httpClientFactory.CreateClient("API").GetFromJsonAsync<FullEmployeeViewModel>($"api/employee/{id}");
    }

    public async Task<FullEmployeeViewModel?> GetFullInfoAboutAuthenticatedEmployeeAsync()
    {
        try
        {
            var response = await _httpClientFactory.CreateClient("API").GetAsync("api/employee/profile");
            response.EnsureSuccessStatusCode();
            var employee = await response.Content.ReadFromJsonAsync<FullEmployeeViewModel>();
            return employee;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error fetching employee profile", ex);
        }
    }

    public async Task<int> CreateEmployeeAsync(EmployeeDto dto)
    {
        var response = await _httpClientFactory.CreateClient("API").PostAsJsonAsync("api/employee", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> UpdateEmployeeAsync(int id, EmployeeDto dto)
    {
        var response = await _httpClientFactory.CreateClient("API").PutAsJsonAsync($"api/employee/update/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> ChangeStatusAsync(int id, EmployeeStatus status)
    {
        var response = await _httpClientFactory.CreateClient("API").PutAsJsonAsync($"api/employee/changeStatus/{id}", status);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}
