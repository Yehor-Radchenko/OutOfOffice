using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Enums;
using OutOfOffice.Common.ViewModels.Employee;

namespace OutOfOffice.UI.Services;

public class EmployeeService
{
    private readonly HttpClient _httpClient;

    public EmployeeService(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("api");
    }

    public async Task<List<TableEmployeeViewModel>> GetTableViewModelsAsync(string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            return await _httpClient.GetFromJsonAsync<List<TableEmployeeViewModel>>($"/employee/table-data?search={search}");
        }
        else
        {
            return await _httpClient.GetFromJsonAsync<List<TableEmployeeViewModel>>("/employee/table-data");
        }
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<FullEmployeeViewModel>($"/employee/{id}");
    }

    public async Task<FullEmployeeViewModel> GetFullInfoAboutAuthenticatedEmployeeAsync()
    {
        return await _httpClient.GetFromJsonAsync<FullEmployeeViewModel>("/employee");
    }

    public async Task<int> CreateEmployeeAsync(EmployeeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/employee", dto);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> UpdateEmployeeAsync(int id, EmployeeDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/employee/update/{id}", dto);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int> ChangeStatusAsync(int id, EmployeeStatus status)
    {
        var response = await _httpClient.PutAsJsonAsync($"/employee/changeStatus/{id}", status);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<int>();
    }
}