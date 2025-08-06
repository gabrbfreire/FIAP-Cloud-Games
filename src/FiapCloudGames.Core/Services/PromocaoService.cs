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

    public async Task<bool> VerificarSePromocaoEstaAtivaAsync(Guid promocaoId, DateTime data)
    {
        var promocao = await _promocaoRepository.BuscarPorIdAsync(promocaoId);
        return promocao?.EstaAtiva(data) ?? false;
    }

    public async Task<bool> RemoverPromocaoAsync(Guid id)
    {
        var promocao = await _promocaoRepository.BuscarPorIdAsync(id);
        if (promocao == null)
            return false;

        foreach (var jogo in promocao.Jogos.ToList())
        {
            jogo.Promocoes.Remove(promocao);
            await _jogoRepository.AtualizarAsync(jogo);
        }

        await _promocaoRepository.RemoverAsync(promocao);
        return true;
    }
}