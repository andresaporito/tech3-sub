using FluentAssertions;
using Moq;
using PedidoApi.Application;
using PedidoApi.Domain;

public class PedidoServiceTests
{
    private readonly Mock<IClienteRepository> _clienteRepo = new();
    private readonly Mock<IPedidoRepository> _pedidoRepo = new();

    private PedidoService CreateSut()
        => new PedidoService(_clienteRepo.Object, _pedidoRepo.Object);

    [Fact]
    public async Task CriarAsync_DeveCriarSemDesconto_QuandoValorAte1000()
    {
        _clienteRepo.Setup(x => x.ExisteAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _pedidoRepo.Setup(x => x.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123);

        var sut = CreateSut();
        var id = await sut.CriarAsync(new CriarPedidoRequest(10, 500m), CancellationToken.None);

        id.Should().Be(123);
        _pedidoRepo.Verify(x => x.AddAsync(It.Is<Pedido>(p => p.ValorFinal == 500m), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarComDesconto_QuandoValorMaiorQue1000()
    {
        _clienteRepo.Setup(x => x.ExisteAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _pedidoRepo.Setup(x => x.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = CreateSut();
        await sut.CriarAsync(new CriarPedidoRequest(10, 2000m), CancellationToken.None);

        _pedidoRepo.Verify(x => x.AddAsync(It.Is<Pedido>(p => p.ValorFinal == 1800m), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_DeveFalhar_QuandoValorInvalido()
    {
        var sut = CreateSut();
        var act = () => sut.CriarAsync(new CriarPedidoRequest(10, 0m), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*maior que zero*");

        _pedidoRepo.Verify(x => x.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CriarAsync_DeveFalhar_QuandoClienteNaoExiste()
    {
        _clienteRepo.Setup(x => x.ExisteAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var sut = CreateSut();
        var act = () => sut.CriarAsync(new CriarPedidoRequest(999, 10m), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*Cliente não encontrado*");

        _pedidoRepo.Verify(x => x.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}