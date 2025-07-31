using FiapCloudGames.Core.Enums;

namespace FiapCloudGames.Core.Entities;

public class Jogo
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public GeneroDoJogoEnum Genero { get; private set; }
    public decimal Preco { get; private set; }

    public ICollection<Promocao> Promocoes { get; private set; }

    protected Jogo() { }

    public Jogo(string titulo, string descricao, GeneroDoJogoEnum genero, decimal preco)
    {
        if (preco < 0)
            throw new ArgumentException("O preço não pode ser negativo.");

        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Genero = genero;
        Preco = preco;
        Promocoes = new List<Promocao>();
    }

    public void AplicarPromocao(Promocao promocao)
    {
        if (!Promocoes.Contains(promocao))
            Promocoes.Add(promocao);
    }

    public decimal CalcularPrecoComDesconto(DateTime dataAtual)
    {
        var promocaoMaisGenerosa = Promocoes
            .Where(p => p.EstaAtiva(dataAtual))
            .OrderByDescending(p => p.PercentualDeDesconto)
            .FirstOrDefault();

        if (promocaoMaisGenerosa is null)
            return Preco;

        var desconto = Preco * (promocaoMaisGenerosa.PercentualDeDesconto / 100m);
        return Preco - desconto;
    }
}