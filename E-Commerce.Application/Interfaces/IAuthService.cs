using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponse<UserProfileDto>> GetUserProfileAsync(string userId);
        Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
    }
}