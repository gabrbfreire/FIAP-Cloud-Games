using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Enums;
using FiapCloudGames.Core.Interfaces.Repositories;
using FiapCloudGames.Core.Services;
using Moq;
using Xunit;

namespace FiapCloudGames.Test.Services;

public class JogoServiceTests
{
    private readonly Mock<IJogoRepository> _mockRepo;
    private readonly JogoService _service;

    public JogoServiceTests()
    {
        _mockRepo = new Mock<IJogoRepository>();
        _service = new JogoService(_mockRepo.Object);
    }

    [Fact]
    public async Task BuscarTodosAsync_DeveRetornarTodosJogos()
    {
        // Arrange
        var jogos = new List<Jogo>
        {
            new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100),
            new Jogo("Jogo 2", "Desc 2", GeneroDoJogoEnum.Aventura, 200)
        };

        _mockRepo.Setup(x => x.BuscarTodosAsync()).ReturnsAsync(jogos);

        // Act
        var result = await _service.BuscarTodosAsync();

        // Assert
        Assert.Equal(2, result.Count());
        _mockRepo.Verify(x => x.BuscarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task BuscarPorIdAsync_QuandoJogoExiste_DeveRetornarJogo()
    {
        // Arrange
        var jogo = new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100);
        _mockRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(jogo);

        // Act
        var result = await _service.BuscarPorIdAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jogo.Titulo, result.Titulo);
    }

    [Fact]
    public async Task BuscarPorIdAsync_QuandoJogoNaoExiste_DeveRetornarNull()
    {
        // Arrange
        _mockRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Jogo?)null);

        // Act
        var result = await _service.BuscarPorIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarJogo()
    {
        // Arrange
        var jogo = new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100);
        _mockRepo.Setup(x => x.AdicionarAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.AdicionarAsync(jogo);

        // Assert
        Assert.Equal(jogo, result);
        _mockRepo.Verify(x => x.AdicionarAsync(jogo), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarJogo()
    {
        // Arrange
        var jogo = new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100);
        _mockRepo.Setup(x => x.AtualizarAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        // Act
        await _service.AtualizarAsync(jogo);

        // Assert
        _mockRepo.Verify(x => x.AtualizarAsync(jogo), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_QuandoJogoExiste_DeveRemover()
    {
        // Arrange
        var jogo = new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100);
        _mockRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(jogo);
        _mockRepo.Setup(x => x.RemoverAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        // Act
        await _service.RemoverAsync(Guid.NewGuid());

        // Assert
        _mockRepo.Verify(x => x.RemoverAsync(jogo), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_QuandoJogoNaoExiste_DeveLancarExcecao()
    {
        // Arrange
        _mockRepo.Setup(x => x.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Jogo?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RemoverAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task BuscarJogosComPrecoDeDescontoAsync_QuandoTemPromocao_DeveAplicarDesconto()
    {
        // Arrange
        var jogo = new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100);
        var promocao = new Promocao("Promo", 50, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
        jogo.Promocoes.Add(promocao);

        _mockRepo.Setup(x => x.BuscarTodosAsync()).ReturnsAsync(new List<Jogo> { jogo });

        // Act
        var result = await _service.BuscarJogosComPrecoDeDescontoAsync(DateTime.Now);

        // Assert
        var jogoResult = result.First();
        Assert.Equal(50m, jogoResult.Preco);
    }

    [Fact]
    public async Task BuscarJogosComPrecoDeDescontoAsync_QuandoNaoTemPromocao_DeveManherPreco()
    {
        // Arrange
        var jogo = new Jogo("Jogo 1", "Desc 1", GeneroDoJogoEnum.Acao, 100);
        _mockRepo.Setup(x => x.BuscarTodosAsync()).ReturnsAsync(new List<Jogo> { jogo });

        // Act
        var result = await _service.BuscarJogosComPrecoDeDescontoAsync(DateTime.Now);

        // Assert
        var jogoResult = result.First();
        Assert.Equal(100m, jogoResult.Preco);
    }
}