using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.DTOs.Auth;

public class TokenDto
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}