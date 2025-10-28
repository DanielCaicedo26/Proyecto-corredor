using Entity.Dtos.Auth;

namespace Bussines.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RegisterWithPersonaAsync(RegisterRequestExtended request);
        string GenerateJwtToken(int userId, string username);
    }
}
