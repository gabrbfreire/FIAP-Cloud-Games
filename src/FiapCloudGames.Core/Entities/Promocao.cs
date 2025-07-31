namespace FiapCloudGames.Core.Entities;

public class Promocao
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal PercentualDeDesconto { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }

    public ICollection<Jogo> Jogos { get; set; }

    protected Promocao() { }

    public Promocao(string nome, decimal percentualDeDesconto, DateTime dataInicio, DateTime dataFim)
    {
        if (percentualDeDesconto < 0 || percentualDeDesconto > 100)
            throw new ArgumentException("O desconto deve estar entre 0% e 100%.");

        Id = Guid.NewGuid();
        Nome = nome;
        PercentualDeDesconto = percentualDeDesconto;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Jogos = new List<Jogo>();
    }

    public bool EstaAtiva(DateTime data) => data >= DataInicio && data <= DataFim;
}