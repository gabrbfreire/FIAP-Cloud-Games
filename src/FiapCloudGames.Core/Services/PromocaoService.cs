using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Interfaces.Repositories;
using FiapCloudGames.Core.Interfaces.Services;
using FiapCloudGames.Infra.Repositories;

namespace FiapCloudGames.Core.Services;

public class PromocaoService : IPromocaoService
{
    private readonly IPromocaoRepository _promocaoRepository;
    private readonly IJogoRepository _jogoRepository;

    public PromocaoService(IPromocaoRepository promocaoRepository, IJogoRepository jogoRepository)
    {
        _promocaoRepository = promocaoRepository;
        _jogoRepository = jogoRepository;
    }

    public async Task<Promocao> CriarPromocaoAsync(string nome, int percentualDeDesconto, DateTime dataInicio, DateTime dataFim)
    {
        var promocao = new Promocao(nome, percentualDeDesconto, dataInicio, dataFim);
        await _promocaoRepository.AdicionarAsync(promocao);
        return promocao;
    }

    public async Task<Promocao?> ObterPromocaoPorIdAsync(Guid id)
    {
        return await _promocaoRepository.BuscarPorIdAsync(id);
    }

    public async Task<IEnumerable<Promocao>> ListarPromocoesAtivasAsync(DateTime dataReferencia)
    {
        return await _promocaoRepository.BuscarAtivasPorDataAsync(dataReferencia);
    }

    public async Task AdicionarJogoAPromocaoAsync(Guid promocaoId, Guid jogoId)
    {
        var promocao = await _promocaoRepository.BuscarPorIdAsync(promocaoId);
        var jogo = await _jogoRepository.BuscarPorIdAsync(jogoId);

        if (promocao == null || jogo == null)
            throw new ArgumentException("Promoção ou Jogo não encontrado.");

        if (!promocao.Jogos.Any(j => j.Id == jogoId))
        {
            promocao.Jogos.Add(jogo);
            jogo.Promocoes.Add(promocao);
            await _promocaoRepository.AtualizarAsync(promocao);
            await _jogoRepository.AtualizarAsync(jogo);
        }
    }

    public async Task RemoverJogoDaPromocaoAsync(Guid promocaoId, Guid jogoId)
    {
        var promocao = await _promocaoRepository.BuscarPorIdAsync(promocaoId);
        var jogo = await _jogoRepository.BuscarPorIdAsync(jogoId);

        if (promocao == null || jogo == null)
            throw new ArgumentException("Promoção ou Jogo não encontrado.");

        var jogoNaPromocao = promocao.Jogos.FirstOrDefault(j => j.Id == jogoId);
        var promocaoNoJogo = jogo.Promocoes.FirstOrDefault(p => p.Id == promocaoId);

        if (jogoNaPromocao != null)
        {
            promocao.Jogos.Remove(jogoNaPromocao);
            await _promocaoRepository.AtualizarAsync(promocao);
        }

        if (promocaoNoJogo != null)
        {
            jogo.Promocoes.Remove(promocaoNoJogo);
            await _jogoRepository.AtualizarAsync(jogo);
        }
    }

    public async Task<IEnumerable<Jogo>> ListarJogosEmPromocaoAsync(Guid promocaoId)
    {
        return await _promocaoRepository.BuscarJogosPorPromocaoIdAsync(promocaoId);
    }

    public async Task<bool> VerificarSePromocaoEstaAtivaAsync(Guid promocaoId, DateTime data)
    {
        var promocao = await _promocaoRepository.BuscarPorIdAsync(promocaoId);
        return promocao?.EstaAtiva(data) ?? false;
    }

    public async Task<decimal> CalcularPrecoComDescontoAsync(Guid jogoId, DateTime data)
    {
        var jogo = await _jogoRepository.BuscarPorIdAsync(jogoId);
        if (jogo == null) throw new ArgumentException("Jogo não encontrado.");

        var jogoComPromocoes = await _jogoRepository.BuscarPorIdAsync(jogoId);
        var promocoesAtivas = jogoComPromocoes?.Promocoes?.Where(p => p.EstaAtiva(data)).ToList();

        if (promocoesAtivas == null || !promocoesAtivas.Any())
            return jogo.Preco;

        var maiorDesconto = promocoesAtivas.Max(p => p.PercentualDeDesconto);
        return jogo.Preco * (1 - maiorDesconto / 100);
    }
}