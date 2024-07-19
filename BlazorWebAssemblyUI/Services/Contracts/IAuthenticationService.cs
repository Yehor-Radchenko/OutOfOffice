using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ResponseModels;

namespace BlazorWebAssemblyUI.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthResponse?> LoginAsync(LoginDto model);
        Task LogoutAsync();
    }
}