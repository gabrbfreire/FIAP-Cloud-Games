using FiapCloudGames.Core.Enums;

namespace FiapCloudGames.Core.Entities;

public class Jogo
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public GeneroDoJogoEnum Genero { get; set; }
    public decimal Preco { get; set; }

    public ICollection<Promocao> Promocoes { get; set; }

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
}