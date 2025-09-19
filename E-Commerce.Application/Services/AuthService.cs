using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("Invalid email or password", null, 401);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("Invalid email or password", null, 401);
            }

            // Update last login time
            user.LastLoginTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = await GenerateJwtTokenAsync(user);
            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return ApiResponse<AuthResponseDto>.SuccessResult(response, "Login successful", 200);
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("Email is already registered", null, 409);
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return ApiResponse<AuthResponseDto>.ErrorResult("Registration failed", errors, 400);
            }

            // Add Customer role to new users
            await _userManager.AddToRoleAsync(user, "Customer");

            var token = await GenerateJwtTokenAsync(user);
            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = new List<string> { "Customer" },
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return ApiResponse<AuthResponseDto>.SuccessResult(response, "Registration successful", 201);
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var issuer = jwtSection["Issuer"] ?? "ECommerceIssuer";
            var audience = jwtSection["Audience"] ?? "ECommerceAudience";
            var key = jwtSection["Key"] ?? "VerySecretDevelopmentKey_ChangeMe";

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}