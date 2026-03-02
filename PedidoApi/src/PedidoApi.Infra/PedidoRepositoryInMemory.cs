using PedidoApi.Domain;

namespace PedidoApi.Infra;

public class PedidoRepositoryInMemory : IPedidoRepository
{
    private static int _seq = 0;
    private static readonly List<Pedido> _db = new();

    public Task<int> AddAsync(Pedido pedido, CancellationToken ct)
    {
        var id = Interlocked.Increment(ref _seq);
        pedido.SetId(id);
        _db.Add(pedido);
        return Task.FromResult(id);
    }
}