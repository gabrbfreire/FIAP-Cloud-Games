using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.DTOs.Request;

public class AdicionarRemoverJogoPromocaoDto
{
    [Required(ErrorMessage = "O ID da promoção é obrigatório.")]
    public Guid PromocaoId { get; set; }

    [Required(ErrorMessage = "O ID do jogo é obrigatório.")]
    public Guid JogoId { get; set; }
}