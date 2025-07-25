using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Interfaces;
using FiapCloudGames.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FiapCloudGames.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager,
                 RoleManager<IdentityRole> roleManager,
                 ILogger<AuthController> logger,
                 ITokenService tokenService, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupDto model)
    {
        try
        {
            var existingUser = await _userManager.FindByNameAsync(model.Email);
            if (existingUser != null)
                return BadRequest("User already exists");

            if ((await _roleManager.RoleExistsAsync(Roles.User)) == false)
            {
                var roleResult = await _roleManager
                      .CreateAsync(new IdentityRole(Roles.User));

                if (roleResult.Succeeded == false)
                {
                    var roleErros = roleResult.Errors.Select(e => e.Description);
                    _logger.LogError($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                    return BadRequest($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                }
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(user, model.Password);

            if (createUserResult.Succeeded == false)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                _logger.LogError(
                    $"Failed to create user. Errors: {string.Join(", ", errors)}"
                );
                return BadRequest($"Failed to create user. Errors: {string.Join(", ", errors)}");
            }

            var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Roles.User);

            if (addUserToRoleResult.Succeeded == false)
            {
                var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                _logger.LogError($"Failed to add role to the user. Errors : {string.Join(",", errors)}");
            }

            return CreatedAtAction(nameof(Signup), null);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginModel)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            
            if (user == null) return BadRequest("User with this username is not registered with us.");

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            if (isValidPassword == false) return Unauthorized();

            List<Claim> authClaims = [
                new (ClaimTypes.Name, loginModel.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = _tokenService.GenerateAccessToken(authClaims);
            string refreshToken = _tokenService.GenerateRefreshToken();
            var tokenInfo = _context.TokenInfos.FirstOrDefault(a => a.Username == user.UserName);

            if (tokenInfo == null)
            {
                var ti = new TokenInfo
                {
                    Username = user.UserName,
                    RefreshToken = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddDays(7)
                };
                _context.TokenInfos.Add(ti);
            } else {
                tokenInfo.RefreshToken = refreshToken;
                tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
            }

            await _context.SaveChangesAsync();

            return Ok(new TokenDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Unauthorized();
        }
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> Refresh(TokenDto tokenDto)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var username = principal.Identity.Name;

            var tokenInfo = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
            if (tokenInfo == null
                || tokenInfo.RefreshToken != tokenDto.RefreshToken
                || tokenInfo.ExpiredAt <= DateTime.UtcNow)
            {
                return BadRequest("Invalid refresh token. Please login again.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            tokenInfo.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
