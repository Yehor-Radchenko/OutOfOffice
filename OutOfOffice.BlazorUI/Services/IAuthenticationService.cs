using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ResponseModels;

namespace OutOfOffice.BlazorUI.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResponse?> LoginAsync(LoginDto model);
        Task LogoutAsync();
    }
}