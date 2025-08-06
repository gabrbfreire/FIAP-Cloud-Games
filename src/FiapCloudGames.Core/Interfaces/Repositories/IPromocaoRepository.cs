using FiapCloudGames.Core.Entities;

namespace FiapCloudGames.Infra.Repositories;

public interface IPromocaoRepository
{
    Task<IEnumerable<Promocao>> BuscarTodasAsync();
    Task<Promocao?> BuscarPorIdAsync(Guid id);
    Task AdicionarAsync(Promocao promocao);
    Task AtualizarAsync(Promocao promocao);
    Task RemoverAsync(Promocao promocao);
    Task<IEnumerable<Promocao>> BuscarAtivasPorDataAsync(DateTime dataReferencia);
}