using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Enums;

namespace FiapCloudGames.API.DTOs.Response;

public class JogoDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public GeneroDoJogoEnum Genero { get; set; }
    public decimal Preco { get; set; }
    public List<PromocaoSimpleDTO> Promocoes { get; set; } = new List<PromocaoSimpleDTO>();

    public JogoDTO()
    {
    }

    public JogoDTO(Jogo jogo)
    {
        Id = jogo.Id;
        Titulo = jogo.Titulo;
        Descricao = jogo.Descricao;
        Genero = jogo.Genero;
        Preco = jogo.Preco;
        Promocoes = jogo.Promocoes?.Select(p => new PromocaoSimpleDTO(p)).ToList() ?? new List<PromocaoSimpleDTO>();
    }
}

public class JogoSimpleDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public decimal Preco { get; set; }

    public JogoSimpleDTO()
    {
    }

    public JogoSimpleDTO(Jogo jogo)
    {
        Id = jogo.Id;
        Titulo = jogo.Titulo;
        Preco = jogo.Preco;
    }
}