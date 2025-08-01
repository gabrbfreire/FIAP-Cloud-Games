using FiapCloudGames.Core.Entities;
using FiapCloudGames.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infra.Repositories;

public class PromocaoRepository : IPromocaoRepository
{
    private readonly AppDbContext _context;

    public PromocaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Promocao>> BuscarTodasAsync()
    {
        return await _context.Promocoes
            .Include(p => p.Jogos)
            .ToListAsync();
    }

    public async Task<Promocao?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Promocoes
            .Include(p => p.Jogos)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AdicionarAsync(Promocao promocao)
    {
        await _context.Promocoes.AddAsync(promocao);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Promocao promocao)
    {
        _context.Promocoes.Update(promocao);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Promocao promocao)
    {
        _context.Promocoes.Remove(promocao);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Promocao>> BuscarAtivasPorDataAsync(DateTime dataReferencia)
    {
        return await _context.Promocoes
            .Include(p => p.Jogos)
            .Where(p => p.DataInicio <= dataReferencia && p.DataFim >= dataReferencia)
            .ToListAsync();
    }

    public async Task<IEnumerable<Jogo>> BuscarJogosPorPromocaoIdAsync(Guid promocaoId)
    {
        var promocao = await _context.Promocoes
            .Include(p => p.Jogos)
            .FirstOrDefaultAsync(p => p.Id == promocaoId);

        return promocao?.Jogos ?? Enumerable.Empty<Jogo>();
    }
}