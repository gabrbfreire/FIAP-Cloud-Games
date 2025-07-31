using System.ComponentModel.DataAnnotations;
using FiapCloudGames.Core.Enums;

namespace FiapCloudGames.API.DTOs;

public class CadastrarJogoDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(150, ErrorMessage = "A descrição deve ter no máximo 150 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0, 5, ErrorMessage = "Gênero inválido. Os valores permitidos são: 0 - Ação, 1 - Aventura, 2 - RPG, 3 - Estratégia, 4 - Simulação, 5 - Esportes.")]
    public GeneroDoJogoEnum Genero { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; set; }
}
