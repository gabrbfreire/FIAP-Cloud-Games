using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Interfaces.Repositories;
using FiapCloudGames.Core.Interfaces.Services;

namespace FiapCloudGames.Core.Services;

public class JogoService : IJogoService
{
    private readonly IJogoRepository _jogoRepository;

    public JogoService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task<IEnumerable<Jogo>> BuscarTodosAsync()
    {
        return await _jogoRepository.BuscarTodosAsync();
    }

    public async Task<Jogo?> BuscarPorIdAsync(Guid id)
    {
        return await _jogoRepository.BuscarPorIdAsync(id);
    }

    public async Task<Jogo> AdicionarAsync(Jogo jogo)
    {
        await _jogoRepository.AdicionarAsync(jogo);
        return jogo;
    }

    public async Task AtualizarAsync(Jogo jogo)
    {
        await _jogoRepository.AtualizarAsync(jogo);
    }

    public async Task RemoverAsync(Guid id)
    {
        var jogo = await _jogoRepository.BuscarPorIdAsync(id);
        if (jogo is null)
            throw new KeyNotFoundException("Jogo não encontrado.");

        await _jogoRepository.RemoverAsync(jogo);
    }

    public async Task<IEnumerable<Jogo>> BuscarJogosComPrecoDeDescontoAsync(DateTime dataReferencia)
    {
        var jogos = await _jogoRepository.BuscarTodosAsync();

        foreach (var jogo in jogos)
            CalcularPrecoComDesconto(dataReferencia, jogo);

        return jogos;
    }

    private void CalcularPrecoComDesconto(DateTime dataAtual, Jogo jogo)
    {
        if (jogo.Promocoes is null) return;

        var promocaoMaisGenerosa = jogo.Promocoes
            .Where(p => p.EstaAtiva(dataAtual))
            .OrderByDescending(p => p.PercentualDeDesconto)
            .FirstOrDefault();

        if (promocaoMaisGenerosa is null) return;

        jogo.Preco = jogo.Preco * (promocaoMaisGenerosa.PercentualDeDesconto / 100m);
    }
}