namespace PedidoApi.Domain;

public interface IPedidoRepository
{
    Task<int> AddAsync(Pedido pedido, CancellationToken ct);
}