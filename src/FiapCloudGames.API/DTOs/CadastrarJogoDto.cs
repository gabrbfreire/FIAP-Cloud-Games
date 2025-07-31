using FiapCloudGames.Core.Enums;

namespace FiapCloudGames.API.DTOs;

public class CadastrarJogoDto
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public GeneroDoJogoEnum Genero { get; set; }
    public decimal Preco { get; set; }
}
