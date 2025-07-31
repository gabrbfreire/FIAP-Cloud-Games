using FiapCloudGames.Core.Entities;

namespace FiapCloudGames.Core.Interfaces.Services;

public interface IJogoService
{
    Task<IEnumerable<Jogo>> BuscarTodosAsync();
    Task<Jogo?> BuscarPorIdAsync(Guid id);
    Task<Jogo> AdicionarAsync(Jogo jogo);
    Task AtualizarAsync(Jogo jogo);
    Task RemoverAsync(Guid id);
    Task<IEnumerable<Jogo>> BuscarJogosComPrecoDeDescontoAsync(DateTime dataReferencia);
}