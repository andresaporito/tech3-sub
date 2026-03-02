namespace PedidoApi.Domain;

public class Pedido
{
    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public decimal ValorOriginal { get; private set; }
    public decimal ValorFinal { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public Pedido(int clienteId, decimal valorOriginal, decimal valorFinal)
    {
        ClienteId = clienteId;
        ValorOriginal = valorOriginal;
        ValorFinal = valorFinal;
        CriadoEm = DateTime.UtcNow;
    }

    public void SetId(int id) => Id = id;
}