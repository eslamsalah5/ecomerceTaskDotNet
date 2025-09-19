using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto loginDto)
        {

            var result = await _authService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(RegisterDto registerDto)
        {

            var result = await _authService.RegisterAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}