using FiapCloudGames.Core.Entities.Identity;
using FiapCloudGames.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FiapCloudGames.Core.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager,
                 RoleManager<IdentityRole> roleManager,
                 ITokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    public async Task<IdentityResult> SignupAsync(string name, string email, string password)
    {
        var existingUser = await _userManager.FindByNameAsync(email);
        if (existingUser != null) return null;

        ApplicationUser user = new()
        {
            Name = name,
            UserName = email,
            Email = email,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true
        };

        return await _userManager.CreateAsync(user, password);
    }

    public async Task<TokenInfo?> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        bool isValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (user == null || !isValidPassword) return null;

        List<Claim> authClaims = [
            new (ClaimTypes.Name, email), new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var userRole in userRoles)
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));

        var token = _tokenService.GenerateAccessToken(authClaims);

        return new TokenInfo(token);
    }

    public async Task<IdentityResult> DeleteAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        return await _userManager.DeleteAsync(user);
    }
}