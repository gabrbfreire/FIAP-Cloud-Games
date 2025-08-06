using FiapCloudGames.API.DTOs.Auth;
using FiapCloudGames.Core.Entities.Identity;
using FiapCloudGames.Core.Interfaces.Identity;
using FiapCloudGames.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;
    private readonly AppDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager,
                 RoleManager<IdentityRole> roleManager,
                 ILogger<AuthController> logger,
                 ITokenService tokenService,
                 AppDbContext context,
                 IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _tokenService = tokenService;
        _context = context;
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupDto dto)
    {
        var signup = await _authService.SignupAsync(dto.Name, dto.Email, dto.Password);

        if (signup != null)
            return CreatedAtAction(nameof(Signup), null);
        else
            return BadRequest("User already exists");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var tokenInfo = await _authService.LoginAsync(dto.Email, dto.Password);

        if (tokenInfo != null)
            return Ok(new TokenDto
            {
                AccessToken = tokenInfo.Token
            });
        else
            return BadRequest("Invalid email or password");
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUserDto dto)
    {
        var tokenInfo = await _authService.DeleteAsync(dto.Email);

        if (tokenInfo != null)
            return Ok();
        else
            return BadRequest("Invalid email");
    }
}
