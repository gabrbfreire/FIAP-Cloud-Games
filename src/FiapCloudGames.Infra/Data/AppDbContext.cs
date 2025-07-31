using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infra.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<TokenInfo> TokenInfos { get; set; }
    public DbSet<Jogo> Jogos { get; set; }
    public DbSet<Promocao> Promocoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Jogo>()
            .HasMany(j => j.Promocoes)
            .WithMany(p => p.Jogos);
    }
}