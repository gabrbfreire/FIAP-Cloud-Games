using FiapCloudGames.Core.Entities;

namespace FiapCloudGames.API.DTOs.Response;

 public class PromocaoDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int PercentualDeDesconto { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public List<JogoSimpleDTO> Jogos { get; set; } = new List<JogoSimpleDTO>();
    public bool EstaAtiva { get; set; }

    public PromocaoDTO()
    {
    }

    public PromocaoDTO(Promocao promocao)
    {
        Id = promocao.Id;
        Nome = promocao.Nome;
        PercentualDeDesconto = promocao.PercentualDeDesconto;
        DataInicio = promocao.DataInicio;
        DataFim = promocao.DataFim;
        Jogos = promocao.Jogos?.Select(j => new JogoSimpleDTO(j)).ToList() ?? new List<JogoSimpleDTO>();
        EstaAtiva = promocao.EstaAtiva(DateTime.Now);
    }
}

public class PromocaoSimpleDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int PercentualDeDesconto { get; set; }

    public PromocaoSimpleDTO()
    {
    }

    public PromocaoSimpleDTO(Promocao promocao)
    {
        Id = promocao.Id;
        Nome = promocao.Nome;
        PercentualDeDesconto = promocao.PercentualDeDesconto;
    }
}