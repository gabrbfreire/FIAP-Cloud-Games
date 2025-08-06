using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.DTOs.Auth;

public class DeleteUserDto
{
    [Required]
    [MaxLength(30)]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;
}
