using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.DTOs.Auth;

public class LoginDto
{
    [Required]
    [MaxLength(30)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Password { get; set; } = string.Empty;
}