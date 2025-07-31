using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Interfaces.Repositories;
using FiapCloudGames.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infra.Repositories;

public class JogoRepository : IJogoRepository
{
    private readonly AppDbContext _context;

    public JogoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Jogo>> BuscarTodosAsync()
    {
        return await _context.Jogos
            .Include(j => j.Promocoes)
            .ToListAsync();
    }

    public async Task<Jogo?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Jogos
            .Include(j => j.Promocoes)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task AdicionarAsync(Jogo jogo)
    {
        await _context.Jogos.AddAsync(jogo);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Jogo jogo)
    {
        _context.Jogos.Update(jogo);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Jogo jogo)
    {
        _context.Jogos.Remove(jogo);
        await _context.SaveChangesAsync();
    }
}