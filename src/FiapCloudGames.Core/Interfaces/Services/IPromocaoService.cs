using FiapCloudGames.Core.Entities;

namespace FiapCloudGames.Core.Interfaces.Services;

public interface IPromocaoService
{
    Task<Promocao> CriarPromocaoAsync(string nome, int percentualDeDesconto, DateTime dataInicio, DateTime dataFim);
    Task<Promocao> ObterPromocaoPorIdAsync(Guid id);
    Task<IEnumerable<Promocao>> ListarPromocoesAtivasAsync(DateTime dataReferencia);
    Task AdicionarJogoAPromocaoAsync(Guid promocaoId, Guid jogoId);
    Task RemoverJogoDaPromocaoAsync(Guid promocaoId, Guid jogoId);
    Task<IEnumerable<Jogo>> ListarJogosEmPromocaoAsync(Guid promocaoId);
    Task<bool> VerificarSePromocaoEstaAtivaAsync(Guid promocaoId, DateTime data);
    Task<decimal> CalcularPrecoComDescontoAsync(Guid jogoId, DateTime data);
}