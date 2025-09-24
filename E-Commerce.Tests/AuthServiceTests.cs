using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace E_Commerce.Tests
{
    public class AuthServiceTests
    {
        private readonly FakeUserManager _userManager;
        private readonly FakeSignInManager _signInManager;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>().Object;
            _userManager = new FakeUserManager(userStore);
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object;
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>().Object;
            var logger = new Mock<ILogger<SignInManager<ApplicationUser>>>().Object;
            var schemes = new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object;
            _signInManager = new FakeSignInManager(_userManager, contextAccessor, claimsFactory, options, logger, schemes);
            _configurationMock = new Mock<IConfiguration>();
            _authService = new AuthService(_userManager, _signInManager, _configurationMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenUserNotFound()
        {
            _userManager.FakeUsers.Clear();
            var loginDto = new LoginDto { Email = "notfound@test.com", Password = "123" };
            var result = await _authService.LoginAsync(loginDto);
            Assert.False(result.Success);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenPasswordIncorrect()
        {
            var user = new ApplicationUser { Email = "user@test.com", PasswordHash = "hashed" };
            _userManager.FakeUsers["user@test.com"] = user;
            _signInManager.PasswordResult = SignInResult.Failed;
            var loginDto = new LoginDto { Email = "user@test.com", Password = "wrong" };
            var result = await _authService.LoginAsync(loginDto);
            Assert.False(result.Success);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenSuccess()
        {
            var user = new ApplicationUser { Email = "user@test.com", FirstName = "Test", LastName = "User" };
            _userManager.FakeUsers["user@test.com"] = user;
            _signInManager.PasswordResult = SignInResult.Success;
            _userManager.FakeRoles["user@test.com"] = new List<string> { "Customer" };
            _configurationMock.Setup(c => c.GetSection("Jwt")).Returns(new MockJwtSection());
            var loginDto = new LoginDto { Email = "user@test.com", Password = "correct" };
            var result = await _authService.LoginAsync(loginDto);
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data.Token);
        }

        [Fact]
        public async Task RefreshTokenAsync_ReturnsError_WhenTokenInvalidOrExpired()
        {
            _userManager.FakeUsers.Clear();
            var result = await _authService.RefreshTokenAsync("invalidtoken");
            Assert.False(result.Success);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task RefreshTokenAsync_ReturnsNewToken_WhenTokenValid()
        {
            var user = new ApplicationUser { Email = "user@test.com", RefreshToken = "validtoken", RefreshTokenExpiry = DateTime.UtcNow.AddDays(1), FirstName = "Test", LastName = "User" };
            _userManager.FakeUsers["user@test.com"] = user;
            _userManager.FakeRoles["user@test.com"] = new List<string> { "Customer" };
            _configurationMock.Setup(c => c.GetSection("Jwt")).Returns(new MockJwtSection());
            var result = await _authService.RefreshTokenAsync("validtoken");
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data.Token);
            Assert.NotNull(result.Data.RefreshToken);
            Assert.NotEqual("validtoken", result.Data.RefreshToken);
        }
    }

// Helper mock for IConfigurationSection
public class MockJwtSection : IConfigurationSection
{
    public string this[string key]
    {
        get
        {
            return key switch
            {
                "Issuer" => "TestIssuer",
                "Audience" => "TestAudience",
                "Key" => "TestKey123456789012345678901234567890",
                _ => null
            };
        }
        set { }
    }
    public string Key => "Jwt";
    public string Path => "Jwt";
    public string Value { get; set; }
    public IEnumerable<IConfigurationSection> GetChildren() => new List<IConfigurationSection>();
    public IChangeToken GetReloadToken() => null;
    public IConfigurationSection GetSection(string key) => this;
}

// Fake UserManager for testing
public class FakeUserManager : UserManager<ApplicationUser>
{
    public Dictionary<string, ApplicationUser> FakeUsers = new();
    public Dictionary<string, List<string>> FakeRoles = new();
    public FakeUserManager(IUserStore<ApplicationUser> store)
        : base(store, null, null, null, null, null, null, null, null) { }
    public override Task<ApplicationUser> FindByEmailAsync(string email)
        => Task.FromResult(FakeUsers.ContainsKey(email) ? FakeUsers[email] : null);
    public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
        => Task.FromResult(IdentityResult.Success);
    public override Task<IList<string>> GetRolesAsync(ApplicationUser user)
        => Task.FromResult((IList<string>)(FakeRoles.ContainsKey(user.Email) ? FakeRoles[user.Email] : new List<string>()));
    public override IQueryable<ApplicationUser> Users => FakeUsers.Values.AsQueryable();
}

// Fake SignInManager for testing
public class FakeSignInManager : SignInManager<ApplicationUser>
{
    public SignInResult PasswordResult = SignInResult.Failed;
    public FakeSignInManager(UserManager<ApplicationUser> userManager,
        Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> options,
        ILogger<SignInManager<ApplicationUser>> logger,
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes)
    : base(userManager, contextAccessor, claimsFactory, options, logger, schemes) { }
    public override Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        => Task.FromResult(PasswordResult);
}
}
