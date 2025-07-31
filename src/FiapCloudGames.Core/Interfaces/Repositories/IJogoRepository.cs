using FiapCloudGames.Core.Entities;

namespace FiapCloudGames.Core.Interfaces.Repositories;

public interface IJogoRepository
{
    Task<IEnumerable<Jogo>> BuscarTodosAsync();
    Task<Jogo?> BuscarPorIdAsync(Guid id);
    Task AdicionarAsync(Jogo jogo);
    Task AtualizarAsync(Jogo jogo);
    Task RemoverAsync(Jogo jogo);
}