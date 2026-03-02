using PedidoApi.Domain;

namespace PedidoApi.Application;

public class PedidoService
{
    private readonly IClienteRepository _clienteRepo;
    private readonly IPedidoRepository _pedidoRepo;

    public PedidoService(IClienteRepository clienteRepo, IPedidoRepository pedidoRepo)
    {
        _clienteRepo = clienteRepo;
        _pedidoRepo = pedidoRepo;
    }

    public async Task<int> CriarAsync(CriarPedidoRequest request, CancellationToken ct)
    {
        if (request.Valor <= 0)
            throw new DomainException("Valor do pedido deve ser maior que zero.");

        var clienteExiste = await _clienteRepo.ExisteAsync(request.ClienteId, ct);
        if (!clienteExiste)
            throw new DomainException("Cliente não encontrado.");

        var valorFinal = request.Valor > 1000m ? request.Valor * 0.90m : request.Valor;

        var pedido = new Pedido(request.ClienteId, request.Valor, valorFinal);
        return await _pedidoRepo.AddAsync(pedido, ct);
    }
}