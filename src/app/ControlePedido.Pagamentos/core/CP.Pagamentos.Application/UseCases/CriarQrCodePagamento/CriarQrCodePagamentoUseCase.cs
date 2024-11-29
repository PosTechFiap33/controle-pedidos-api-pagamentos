using System;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Application.UseCases.CriarQrCodePagamento;

public class CriarQrCodePagamentoUseCase
{
    public Guid PedidoId { get; private set; }
    public List<PedidoItem> Itens { get; private set; }
    
    public CriarQrCodePagamentoUseCase(Guid pedidoId, List<PedidoItem> itens)
    {
        PedidoId = pedidoId;
        Itens = itens;
    }    
}

public class CriarQrCodePagamentoUseCaseResult{
    public string QrCode { get; private set; }

    public CriarQrCodePagamentoUseCaseResult(string qrCode)
    {
        QrCode = qrCode;
    }
}
