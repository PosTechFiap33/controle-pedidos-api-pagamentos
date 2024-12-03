using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Api.DTOs;

public class GeracaoQrCode
{
    public List<PedidoItem> Itens { get; set; }
}
