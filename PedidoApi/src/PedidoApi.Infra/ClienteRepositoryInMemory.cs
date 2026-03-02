using PedidoApi.Domain;

namespace PedidoApi.Infra;

public class ClienteRepositoryInMemory : IClienteRepository
{
    private static readonly HashSet<int> Clientes = new() { 1, 2, 3, 10 };

    public Task<bool> ExisteAsync(int clienteId, CancellationToken ct)
        => Task.FromResult(Clientes.Contains(clienteId));
}