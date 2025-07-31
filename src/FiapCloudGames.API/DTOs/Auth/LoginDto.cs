using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.DTOs.Auth;

public class LoginDto
{
    [Required]
    [MaxLength(30)]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "A senha deve conter letras, números e caracteres especiais.")]
    public string Password { get; set; } = string.Empty;
}