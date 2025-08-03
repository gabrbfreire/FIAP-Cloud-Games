using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Enums;
using FiapCloudGames.Core.Interfaces.Repositories;
using FiapCloudGames.Core.Services;
using FiapCloudGames.Infra.Repositories;
using Moq;
using Xunit;

namespace FiapCloudGames.Test.Services;

public class PromocaoServiceTests
{
    private readonly Mock<IPromocaoRepository> _mockPromocaoRepo;
    private readonly Mock<IJogoRepository> _mockJogoRepo;
    private readonly PromocaoService _service;

    public PromocaoServiceTests()
    {
        _mockPromocaoRepo = new Mock<IPromocaoRepository>();
        _mockJogoRepo = new Mock<IJogoRepository>();
        _service = new PromocaoService(_mockPromocaoRepo.Object, _mockJogoRepo.Object);
    }

    [Fact]
    public async Task CriarPromocaoAsync_DeveCriarPromocao()
    {
        // Arrange
        var promocao = new Promocao("Promo", 10, DateTime.Now, DateTime.Now.AddDays(1));
        _mockPromocaoRepo.Setup(x => x.AdicionarAsync(It.IsAny<Promocao>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CriarPromocaoAsync("Promo", 10, DateTime.Now, DateTime.Now.AddDays(1));

        // Assert
        Assert.Equal("Promo", result.Nome);
        _mockPromocaoRepo.Verify(x => x.AdicionarAsync(It.IsAny<Promocao>()), Times.Once);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task CriarPromocaoAsync_QuandoDescontoInvalido_DeveLancarExcecao(int desconto)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarPromocaoAsync("Promo", desconto, DateTime.Now, DateTime.Now.AddDays(1)));
    }

    [Fact]
    public async Task ObterPromocaoPorIdAsync_QuandoExiste_DeveRetornarPromocao()
    {
        // Arrange
        var promocao = new Promocao("Promo", 10, DateTime.Now, DateTime.Now.AddDays(1));
        _mockPromocaoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(promocao);

        // Act
        var result = await _service.ObterPromocaoPorIdAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Promo", result.Nome);
    }

    [Fact]
    public async Task ListarPromocoesAtivasAsync_DeveRetornarPromocoesAtivas()
    {
        // Arrange
        var promocoes = new List<Promocao>
        {
            new Promocao("Promo 1", 10, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)),
            new Promocao("Promo 2", 20, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2))
        };

        _mockPromocaoRepo.Setup(x => x.BuscarAtivasPorDataAsync(It.IsAny<DateTime>())).ReturnsAsync(promocoes);

        // Act
        var result = await _service.ListarPromocoesAtivasAsync(DateTime.Now);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AdicionarJogoAPromocaoAsync_QuandoPromocaoEJogoExistem_DeveAdicionar()
    {
        // Arrange
        var promocao = new Promocao("Promo", 10, DateTime.Now, DateTime.Now.AddDays(1));
        var jogo = new Jogo("Jogo", "Desc", GeneroDoJogoEnum.Acao, 100);

        _mockPromocaoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(promocao);
        _mockJogoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(jogo);
        _mockPromocaoRepo.Setup(x => x.AtualizarAsync(It.IsAny<Promocao>())).Returns(Task.CompletedTask);
        _mockJogoRepo.Setup(x => x.AtualizarAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        // Act
        await _service.AdicionarJogoAPromocaoAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.Single(promocao.Jogos);
        Assert.Single(jogo.Promocoes);
    }

    [Fact]
    public async Task AdicionarJogoAPromocaoAsync_QuandoPromocaoNaoExiste_DeveLancarExcecao()
    {
        // Arrange
        _mockPromocaoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Promocao?)null);
        _mockJogoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Jogo("Jogo", "Desc", GeneroDoJogoEnum.Acao, 100));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AdicionarJogoAPromocaoAsync(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task RemoverJogoDaPromocaoAsync_QuandoExistem_DeveRemover()
    {
        // Arrange
        var jogoId = Guid.NewGuid();
        var promocaoId = Guid.NewGuid();

        var promocao = new Promocao("Promo", 10, DateTime.Now, DateTime.Now.AddDays(1))
        {
            Id = promocaoId
        };

        var jogo = new Jogo("Jogo", "Desc", GeneroDoJogoEnum.Acao, 100)
        {
            Id = jogoId
        };

        promocao.Jogos.Add(jogo);
        jogo.Promocoes.Add(promocao);

        _mockPromocaoRepo.Setup(x => x.BuscarPorIdAsync(promocaoId)).ReturnsAsync(promocao);
        _mockJogoRepo.Setup(x => x.BuscarPorIdAsync(jogoId)).ReturnsAsync(jogo);
        _mockPromocaoRepo.Setup(x => x.AtualizarAsync(It.IsAny<Promocao>())).Returns(Task.CompletedTask);
        _mockJogoRepo.Setup(x => x.AtualizarAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        // Act
        await _service.RemoverJogoDaPromocaoAsync(promocaoId, jogoId);

        // Assert
        Assert.Empty(promocao.Jogos);
        Assert.Empty(jogo.Promocoes);

        // Verifica se os métodos de atualização foram chamados
        _mockPromocaoRepo.Verify(x => x.AtualizarAsync(promocao), Times.Once);
        _mockJogoRepo.Verify(x => x.AtualizarAsync(jogo), Times.Once);
    }

    [Fact]
    public async Task VerificarSePromocaoEstaAtivaAsync_QuandoEstaAtiva_DeveRetornarTrue()
    {
        // Arrange
        var promocao = new Promocao("Promo", 10, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
        _mockPromocaoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(promocao);

        // Act
        var result = await _service.VerificarSePromocaoEstaAtivaAsync(Guid.NewGuid(), DateTime.Now);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerificarSePromocaoEstaAtivaAsync_QuandoNaoEstaAtiva_DeveRetornarFalse()
    {
        // Arrange
        var promocao = new Promocao("Promo", 10, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));
        _mockPromocaoRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(promocao);

        // Act
        var result = await _service.VerificarSePromocaoEstaAtivaAsync(Guid.NewGuid(), DateTime.Now);

        // Assert
        Assert.False(result);
    }
}