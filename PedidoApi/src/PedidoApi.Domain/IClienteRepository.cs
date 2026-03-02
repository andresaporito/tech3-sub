namespace PedidoApi.Domain;

public interface IClienteRepository
{
    Task<bool> ExisteAsync(int clienteId, CancellationToken ct);
}