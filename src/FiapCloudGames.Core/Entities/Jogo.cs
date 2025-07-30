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

    public decimal ObterPrecoComDesconto(DateTime dataAtual)
    {
        var promocaoAtiva = Promocoes.FirstOrDefault(p => p.EstaAtiva(dataAtual));
        if (promocaoAtiva == null)
            return Preco;

        var desconto = Preco * (promocaoAtiva.PercentualDeDesconto / 100);
        return Preco - desconto;
    }
}